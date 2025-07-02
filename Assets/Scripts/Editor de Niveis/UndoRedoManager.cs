using UnityEngine;
using System.Collections.Generic;

public class UndoRedoManager : MonoBehaviour
{
    private Stack<ICommand> undoStack = new Stack<ICommand>();
    private Stack<ICommand> redoStack = new Stack<ICommand>();

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        undoStack.Push(command);
        redoStack.Clear();
    }

    public void Undo()
    {
        if (undoStack.Count > 0)
        {
            ICommand cmd = undoStack.Pop();
            cmd.Undo();
            redoStack.Push(cmd);
        }
    }

    public void Redo()
    {
        if (redoStack.Count > 0)
        {
            ICommand cmd = redoStack.Pop();
            cmd.Execute();
            undoStack.Push(cmd);
        }
    }
}

public interface ICommand
{
    void Execute();
    void Undo();
}
