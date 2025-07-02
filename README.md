# Editor de Fases 3D na Unity — Projeto Insolar

## Visão Geral
Este projeto é uma **ferramenta de edição de cenários 3D** dentro do próprio Play Mode da Unity. Inspirado no Pokémon DS Map Studio e na interface do The Sims 4, o editor permite pintar prefabs, preencher áreas, selecionar e manipular objetos, definir conexões entre mapas e salvar tudo em tempo real.

## Estrutura de Pastas
```
Assets/
  EditorPrefabs/
    Cenarios/       # Prefabs de blocos modulares de cenário
    Props/          # Objetos de decoração (árvores, pedras, móveis)
    Portais/        # Prefabs de portais/conectores entre cenários
    SpawnAreas/     # Prefabs de áreas de encontro (monstros, itens)
    Thumbnails/     # Ícones e thumbnails para UI (importados do Pokemon-DS-Map-Studio)
  Scripts/
    Editor de Niveis/
      BaseTool.cs
      BrushTool.cs
      FillTool.cs
      SelectTool.cs
      PrefabManager.cs
      EditorManager.cs
      UITools.cs
      UndoRedoManager.cs
      RuntimeSaver.cs
      GridRenderer.cs
      AssetImporterWindow.cs
      PrefabIconImporter.cs
  Scenes/
    ...
README.md
```

## Principais Funcionalidades
- **Pincel de Prefab (Brush Tool)**: desenha instâncias de prefabs com espaçamento e snap.
- **Ferramenta de Preenchimento (Fill Tool)**: define área 3D e preenche com múltiplos prefabs.
- **Ferramenta de Seleção (Select Tool)**: seleciona, move, rotaciona, escala e deleta objetos.
- **Ferramenta Mover (Move Tool)**: manipulação de gizmos para posicionamento preciso.
- **Conta-Gotas (Eyedropper Tool)**: captura o prefab de um objeto existente.
- **Borrachinha (Eraser Tool)**: apaga elementos selecionados ou arrastando.
- **Pincel de Decoração (Scatter Brush)**: distribui automaticamente múltiplos objetos pequenos (variação de escala/rotação).
- **Snap to Grid**: grade configurável (ativar/desativar, tamanho ajustável).
- **Undo/Redo**: desfazer/refazer ações (Ctrl+Z / Ctrl+Y).
- **Salvar como Prefab (Runtime Save)**: exporta layout atual em JSON e recria prefab no Editor.
- **Conectores entre Mapas**: adiciona portais com IDs e conexões serializadas.
- **Áreas de Spawn**: define regiões de encontro de monstros ou itens.

## Scripts Principais
- **EditorManager.cs**: Singleton que controla ferramenta ativa, prefab selecionado, snap, comandos de undo/redo e salvamento.
- **UITools.cs**: gerencia Canvas, botões, lista de prefabs com thumbnails, toggle de snap e feedback visual.
- **BaseTool.cs**: classe abstrata base para ferramentas, com raycast e callbacks de entrada.
- **BrushTool.cs / FillTool.cs / SelectTool.cs**: implementações específicas de cada ferramenta.
- **PrefabManager.cs**: carrega e categoriza prefabs de pastas, fornece thumbnails para UI.
- **UndoRedoManager.cs**: padrão Command para registrar e reverter operações.
- **RuntimeSaver.cs**: serializa objetos, conexões e áreas em JSON para persistência.
- **GridRenderer.cs**: desenha grade 3D no chão conforme configuração do EditorManager.
- **AssetImporterWindow.cs**: janela de Editor para drag-and-drop de arquivos `.obj` e conversão em prefabs.
- **PrefabIconImporter.cs**: importa ícones do repositório Pokemon-DS-Map-Studio para a pasta Thumbnails.


## Próximos Passos Detalhados

Aqui estão as próximas etapas detalhadas para o desenvolvimento do Editor de Fases 3D, organizadas por áreas de foco:

### 1. **Interface (UI)**
- **Ícones e Pré-visualizações**:
    - Finalizar a implementação de ícones e prévias reais de prefabs na interface.
    - Garantir que os thumbnails sejam gerados automaticamente ao importar novos prefabs.
- **Feedback Visual**:
    - Adicionar feedback visual para ações como seleção, preenchimento e salvamento.
    - Implementar destaques visuais para objetos selecionados ou áreas de preenchimento.
- **Painel de Configurações**:
    - Criar um painel dedicado para ajustar parâmetros como:
        - Tamanho da grade.
        - Ativação/desativação do snap.
        - Opções de salvamento e carregamento.
    - Adicionar sliders, toggles e campos de entrada para personalização.

### 2. **Ferramentas**
- **Ferramentas Adicionais**:
    - **Conta-Gotas (Eyedropper Tool)**:
        - Implementar funcionalidade para capturar o prefab de um objeto existente na cena.
    - **Borrachinha (Eraser Tool)**:
        - Permitir apagar objetos selecionados ou por arrasto.
    - **Pincel de Dispersão (Scatter Brush)**:
        - Distribuir automaticamente múltiplos objetos pequenos com variação de escala e rotação.
    - **Duplicar e Espelhar (Duplicate & Mirror)**:
        - Criar cópias de objetos e espelhar em eixos específicos.
    - **Elevar/Abaixar (Raise/Lower)**:
        - Ajustar a altura de objetos em relação ao plano da grade.
- **Melhorias nas Ferramentas Existentes**:
    - Refinar o comportamento do pincel para suportar diferentes tamanhos e formas.
    - Adicionar atalhos de teclado para alternar rapidamente entre ferramentas.

### 3. **Fluxo de Salvamento**
- **Serialização e Persistência**:
    - Desenvolver o fluxo completo de salvamento em JSON, incluindo:
        - Serialização de objetos, conexões entre mapas e áreas de spawn.
        - Suporte para salvar e carregar múltiplos layouts de cenários.
    - Garantir que os dados salvos possam ser reconstruídos com precisão no Editor Mode.
- **Interface de Gerenciamento**:
    - Criar uma interface para listar, renomear e excluir layouts salvos.
    - Adicionar opções para exportar/importar arquivos JSON.

### 4. **Grupos e Hierarquias**
- **Agrupamento de Objetos**:
    - Implementar funcionalidade para agrupar objetos em hierarquias.
    - Permitir operações em grupo, como mover, rotacionar e escalar.
- **Interface de Hierarquia**:
    - Adicionar opções para expandir/colapsar grupos na interface.
    - Exibir hierarquias de forma clara e intuitiva no painel de objetos.

### 5. **Otimização de Performance**
- **Pooling de Objetos**:
    - Introduzir pooling para reduzir a criação e destruição de instâncias desnecessárias.
- **Batching**:
    - Implementar otimizações de batch para melhorar o desempenho em cenas com muitos objetos.
- **Cálculos em Tempo Real**:
    - Avaliar e reduzir o impacto de cálculos de grade e raycasting em tempo real.
    - Adicionar opções para ajustar a frequência de atualizações em dispositivos menos potentes.

### 6. **Documentação e Usabilidade**
- **Documentação do Usuário**:
    - Criar uma documentação clara e acessível, incluindo:
        - Lista de atalhos de teclado (R, S, G, Ctrl+Z, Ctrl+Y, etc.).
        - Guia de uso detalhado para cada ferramenta e funcionalidade.
    - Disponibilizar a documentação diretamente no editor ou como um arquivo PDF.
- **Testes de Usabilidade**:
    - Realizar testes com usuários para identificar problemas no fluxo de trabalho.
    - Coletar feedback para ajustes e melhorias.
- **Mensagens de Erro e Dicas**:
    - Adicionar mensagens de erro claras para ações inválidas.
    - Implementar dicas contextuais para guiar o usuário durante o uso do editor.

Esses passos são fundamentais para refinar o editor, garantindo uma experiência mais robusta, intuitiva e eficiente para os usuários. 🚀


## Contribuições
- **Designer/Level Artists**: criar e organizar prefabs nas pastas de EditorPrefabs.
- **Programadores**: expandir e refinar scripts de ferramentas e serialização.
- **Testers**: validar fluxo de edição, salvamento e reconstrução de cenários.

---
Feito com carinho para tornar a criação de mapas rápido, poderoso e divertido dentro da própria Unity! 🚀
