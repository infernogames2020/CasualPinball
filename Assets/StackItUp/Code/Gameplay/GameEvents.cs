using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents 
{
	public const string RELOAD_LEVEL    = "RELOAD_LEVEL";
	public const string SKIP_LEVEL = "SKIP_LEVEL";

	public const string LOAD_NEXT       = "LOAD_NEXT";
	public const string STOP_PLATFORMS  = "STOP_PLATFORMS";


	public const string POP_TILE = "POP_TILE";
	public const string PUSH_TILE = "PUSH_TILE";
	public const string CHECK_COMPLETE = "CHECK_COMPLETE";
	public const string STACK_LOAD_COMPLETE = "STACK_LOAD_COMPLETE";
	public const string SAVE_GAME = "SAVE_GAME";
	public const string SAVE_SETTINGS = "SAVE_SETTINGS";

}

public class UIEvents
{
	public const string RESULT = "result";
	public const string SETTINGS = "settings";
}
