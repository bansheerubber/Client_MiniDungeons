if(!isObject(MD_Map)) {
	exec("./guis/MD_Map.gui");
}

$MD::MapDimensions = "1024 1024"; // dimensions of the map in pixels

function MD_Map::showUI(%this) {
	MD_MapHands.setColor($Pref::Avatar::RHandColor);
	Canvas.pushDialog(MD_Map);
}

function MD_Map::hideUI(%this) {
	Canvas.popDialog(MD_Map);
}

function MD_Map::loadImages(%this, %maxX, %maxY, %width, %height, %size) {
	MD_MapBitmapContainer.deleteAll();
	
	%path = "config/client/MiniDungeons/egg_";

	for(%x = 0; %x <= %maxX; %x++) {
		for(%y = 0; %y <= %maxY; %y++) {
			%bitmap = new GuiBitmapCtrl() {
				position = %x * %size SPC %y * %size;
				extent = %size SPC %size;
				mColor = "255 255 255 255";
				mMultiply = true;
				enabled = true;
				visible = true;
				clipToParent = true;
				minExtent = "8 8";
				bitmap = %path @ %x @ "_" @ %y @ ".png";
			};
			MD_MapBitmapContainer.add(%bitmap);
		}
	}

	$MD::MapDimensions = %width SPC %height;
}

deActivatePackage(MiniDungeonsMap);
package MiniDungeonsMap {
	function GuiMouseEventCtrl::onMouseDragged(%this, %eventModifier, %xy, %numMouseClicks) {
		Parent::onMouseDragged(%this, %eventModifier, %xy, %numMouseClicks);
		
		if(%this == MD_MapMouseControl.getId()) {
			%position = vectorAdd(%this.startMapPosition, vectorSub(%xy, %this.startMousePosition));
			// bound the position
			if(getWord(MD_MapMask.extent, 0) < getWord($MD::MapDimensions, 0)) {
				%x = mClamp(getWord(%position, 0), -getWord($MD::MapDimensions, 0) + getWord(MD_MapMask.extent, 0), 0);
			}
			else {
				%x = -getWord($MD::MapDimensions, 0) / 2 + getWord(MD_MapMask.extent, 0) / 2;
			}

			%y = mClamp(getWord(%position, 1), -getWord($MD::MapDimensions, 1) + getWord(MD_MapMask.extent, 1), 0);
			MD_MapBitmapContainer.position = %x SPC %y;
		}
	}

	function GuiMouseEventCtrl::onMouseDown(%this, %eventModifier, %xy, %numMouseClicks) {
		Parent::onMouseDown(%this, %eventModifier, %xy, %numMouseClicks);
		
		if(%this == MD_MapMouseControl.getId()) {
			%this.startMousePosition = %xy;
			%this.startMapPosition = MD_MapBitmapContainer.position;
		}
	}
};
activatePackage(MiniDungeonsMap);