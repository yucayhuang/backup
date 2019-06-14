MyYaoGan =
{

}

 
local gameObject;
local transform;
local yaogan;
local border;
local radius;
local initPos;
function MyYaoGan:Awake(go)
	
	gameObject = go;
	transform = go.transform;

	go.transform.localPosition = Vector3.zero;

	yaogan = transform:FindChild("yaogan");
	border = transform:FindChild("border");
	initPos = yaogan.position;
	radius = Vector3.Distance(yaogan.position,border.position);


	local function onDrag()
		self:OnDrag();
	end

	local function onEndDrag()
		self:OnEndDrag();
	end

	local listener = EventTriggerListener.Get(yaogan.gameObject);

	listener.luaOnDrag = onDrag;
	listener.luaOnEndDrag = onEndDrag;
end


 
function MyYaoGan:OnDrag()
	local mousePosition = UnityEngine.Input.mousePosition;

	if radius >= Vector3.Distance(initPos,mousePosition) then

		yaogan.position = mousePosition;

	else 

		local vct = mousePosition - initPos;

		yaogan.localPosition = vct.normalized * radius;
	end

end

function MyYaoGan:OnEndDrag()

	yaogan.position = initPos;
end






--创建对象

function MyYaoGan:New(obj)

        local o = {}

       setmetatable(o, self)  

       self.__index = self  

      return o

end  