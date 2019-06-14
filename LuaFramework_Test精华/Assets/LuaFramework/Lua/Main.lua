require "MyLua/MyYaoGan";
--主入口函数。从这里开始lua逻辑
local cube;
local canvas;
function Main()					
	print("logic start")	
	canvas = UnityEngine.GameObject.Find("Canvas");
	cube = UnityEngine.GameObject.CreatePrimitive(PrimitiveType.Cube); 	
	UpdateBeat:Add(update,self);


	local LuaHelper = LuaFramework.LuaHelper;
	local resMgr = LuaHelper.GetResManager();
	
	local function OnLoadComplete(objs)
		
		local bg = UnityEngine.GameObject.Instantiate(objs[0]);
		bg.transform:SetParent(canvas.transform);
		LuaComponent.Add(bg,MyYaoGan);
	end
	
	resMgr:LoadPrefab('prefab',{'BackGround'},OnLoadComplete);
end

function  update()
	cube.transform:Rotate(Vector3.up);
end

--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	Time.timeSinceLevelLoad = 0
end