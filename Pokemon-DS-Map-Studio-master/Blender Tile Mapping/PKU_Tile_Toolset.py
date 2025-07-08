bl_info = {
    "name": "Pokemon Tile Toolset",
    "description": "Toolset for easy creation of map assets for Unity 3D with Pokemon framework",
    "author": "Herbert Milhomme, updated for Blender 4.4 by Copilot",
    "version": (1, 1, 0),
    "blender": (4, 4, 0),
    "location": "3D View > Sidebar > Pokemon Tile Toolset",
    "category": "Object"
}

import bpy
import os
import math
import json
from mathutils import Vector

# -------------------------------------------------------
# Pokemon Tile-Export
class MultiPokeTileExportOperator(bpy.types.Operator):
    """Export Pokemon Tiles to Unity"""
    bl_idname = "object.multi_poke_tile_export"
    bl_label = "Export Pokemon Tiles to Unity"
    bl_options = {'REGISTER', 'UNDO'}

    def convert_ob(self, ob):
        name, vector, euler, material = ob
        # Clean up string representations for vector, euler, material
        vector = vector.replace('<Vector (', '').replace(')>', '')
        euler = euler.replace('<Euler (', '').replace('), order=\'XYZ\'>', '')
        euler = euler.replace('x=', '').replace('y=', '').replace('z=', '')
        material = material.replace('<bpy_struct, Material("', '').replace('")>', '')
        vector = [float(coord) for coord in vector.split(',')]
        euler = [float(coord) for coord in euler.split(',')]
        return {"mesh": name, "vector": vector, "rotation": euler, "material": material}

    def execute(self, context):
        if not bpy.data.filepath:
            self.report({'INFO'}, 'Objects not exported: Blend file is not saved')
            return {'CANCELLED'}

        current_selected_obj = context.selected_objects
        current_unit_system = context.scene.unit_settings.system
        name = context.active_object.name
        context.scene.unit_settings.system = 'METRIC'
        context.scene.unit_settings.scale_length = 1.0

        # Setup export folder
        path = bpy.path.abspath('//JSON/')
        if not os.path.exists(path):
            os.makedirs(path)

        # Deselect all and select only meshes
        bpy.ops.object.select_all(action='DESELECT')
        obs = []
        for x in current_selected_obj:
            if x.type == 'MESH':
                x.select_set(True)
                mat = x.data.materials[0] if x.data.materials else ""
                obs.append([
                    x.data.name,
                    str(x.matrix_world.to_translation()),
                    str(x.matrix_world.to_euler()),
                    str(mat)
                ])
        obs_converted = [self.convert_ob(ob) for ob in obs]

        # Calculate width and height from tile vectors
        if obs_converted:
            width = max(tile["vector"][0] for tile in obs_converted) - min(tile["vector"][0] for tile in obs_converted) + 1
            height = max(tile["vector"][1] for tile in obs_converted) - min(tile["vector"][1] for tile in obs_converted) + 1
            width = int(width)
            height = int(height)
        else:
            width = height = 0

        output_data = {
            "width": width,
            "height": height,
            "tiles": obs_converted
        }
        with open(os.path.join(path, f"{name}.json"), 'w') as wfile:
            json.dump(output_data, wfile, sort_keys=False, indent=4)

        # Re-select previously selected objects
        for obj in current_selected_obj:
            obj.select_set(True)
        context.scene.unit_settings.scale_length = 1.0
        context.scene.unit_settings.system = current_unit_system

        self.report({'INFO'}, f"Exported to {os.path.join(path, f'{name}.json')}")
        return {'FINISHED'}

# -------------------------------------------------------
# Replace Mesh/Material Operator
class ReplaceMeshMaterialOperator(bpy.types.Operator):
    bl_idname = "object.replace_properties_operator"
    bl_label = "Replace Properties"
    bl_options = {'REGISTER', 'UNDO'}

    clear_materials: bpy.props.BoolProperty(
        name="Clear Materials",
        description="If true, clear all materials from the object before assigning the new one",
        default=False
    )

    @classmethod
    def poll(cls, context):
        return context.active_object is not None

    def execute(self, context):
        # Replace mesh if specified
        mesh_name = context.scene.replace_mesh_name
        if mesh_name and mesh_name in bpy.data.meshes:
            source_mesh = bpy.data.meshes[mesh_name]
            reposition = context.scene.replace_mesh_and_reposition_with_offset
            for obj in context.selected_objects:
                if obj.type == 'MESH':
                    if reposition:
                        # Calculate offset and move
                        current_origin_world = obj.matrix_world.to_translation()
                        mesh_bbox_center_object = sum((Vector(b) for b in obj.bound_box), Vector()) / 8
                        mesh_bbox_center_world = obj.matrix_world @ mesh_bbox_center_object
                        offset = mesh_bbox_center_world - current_origin_world
                        offset = Vector((math.floor(coordinate) for coordinate in offset))
                        obj.location += offset
                    obj.data = source_mesh

        # Replace material if specified
        material_name = context.scene.replace_material_name
        if material_name and material_name in bpy.data.materials:
            source_material = bpy.data.materials[material_name]
            for obj in context.selected_objects:
                if obj.type == 'MESH':
                    if self.clear_materials:
                        obj.data.materials.clear()
                    if source_material not in obj.data.materials:
                        obj.data.materials.append(source_material)

        return {'FINISHED'}

# -------------------------------------------------------
# Sidebar Panel for UI
class VIEW3D_PT_ImportExport_PKUTools_panel(bpy.types.Panel):
    bl_label = "Pokemon Tile Toolset"
    bl_idname = "VIEW3D_PT_importexport_pku_tools"
    bl_space_type = "VIEW_3D"
    bl_region_type = "UI"
    bl_category = 'Pokemon Toolset'

    def draw(self, context):
        layout = self.layout
        col = layout.column(align=True)
        if context.object is not None and context.mode == 'OBJECT':
            col.operator("object.multi_poke_tile_export", text="Export Tiles to Unity")
            col.separator()
            col.prop_search(context.scene, 'replace_mesh_name', bpy.data, 'meshes')
            col.prop(context.scene, "replace_mesh_and_reposition_with_offset", text="Calculate Offset")
            col.prop_search(context.scene, 'replace_material_name', bpy.data, 'materials')
            col.prop(ReplaceMeshMaterialOperator, "clear_materials")
            col.operator('object.replace_properties_operator', text="Replace/Assign")

# -------------------------------------------------------
# Registration
classes = [
    MultiPokeTileExportOperator,
    ReplaceMeshMaterialOperator,
    VIEW3D_PT_ImportExport_PKUTools_panel,
]

def register():
    for cls in classes:
        bpy.utils.register_class(cls)

    bpy.types.Scene.replace_mesh_and_reposition_with_offset = bpy.props.BoolProperty(
        name="Calculate Offset",
        description="When changing Mesh, will reposition object location to match the changes",
        default=True
    )
    bpy.types.Scene.replace_mesh_name = bpy.props.StringProperty(
        name="Mesh",
        description="Select a mesh to use",
        default=""
    )
    bpy.types.Scene.replace_material_name = bpy.props.StringProperty(
        name="Material",
        description="Select a material to use",
        default=""
    )

def unregister():
    for cls in reversed(classes):
        bpy.utils.unregister_class(cls)
    del bpy.types.Scene.replace_mesh_and_reposition_with_offset
    del bpy.types.Scene.replace_mesh_name
    del bpy.types.Scene.replace_material_name

if __name__ == "__main__":
    register()