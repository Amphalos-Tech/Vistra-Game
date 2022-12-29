using UnityEngine;
using VistraFileSystem;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class DialogueHandler
{
    private static List<DialogueBlock> dialogueBlocks;
    private static int position = 0;
    static DialogueHandler()
    {
        var path = Application.dataPath + "/";
        DialogueSystem.Path = path;
        if (!File.Exists(path + "encrypted" + SaveHandler.Stage + ".xml")) //DO NOT LEAVE THIS BLOCK IN BUILD!!!!!!!!!!
        {
            List<DialogueBlock> blocks = new List<DialogueBlock>
            {
                new DialogueBlock() { Speaker = "Gamer", Words = "A coroutine is a method that you declare with an IEnumerator return type and with a yield return statement included somewhere in the body. The yield return nullline is the point where execution pauses and resumes in the following frame. To set a coroutine running, you need to use the StartCoroutine function:" }
            };
            DialogueSystem.Write(blocks, SaveHandler.Stage);
        }
    }
    public static DialogueBlock GetNextDialogueBlock()
    {
        if (position < dialogueBlocks.Count)
        {
            return dialogueBlocks[position++];
        }
        else
        {
            position = 0; //Set up for future if multiple dialogues in a session
            return null;
        }
    }

    public static void LoadLines()
    {
        dialogueBlocks = DialogueSystem.Read(SaveHandler.Stage);
    }
}
