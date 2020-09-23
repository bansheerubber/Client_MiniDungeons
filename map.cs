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

function MD_Map::display(%this) {
	%this.loadImages($MD::TileMaxXIndex, $MD::TileMaxYIndex, $MD::TileXResolution, $MD::TileYResolution, $MD::TileSize);
	%this.showUI();
}

function MD_Map::loadImages(%this, %maxX, %maxY, %width, %height, %size) {
	MD_MapBitmapContainer.deleteAll();
	
	%path = "config/client/MiniDungeons/tiles/tile_";

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

function MD_downloadTile(%start) {
	if(%start) {
		$MD::TileCurrentXIndex = 0;
		$MD::TileCurrentYIndex = 0;
		$MD::TileMaxXIndex = 0;
		$MD::TileMaxYIndex = 0;
		$MD::TileXResolution = 0;
		$MD::TileYResolution = 0;
		$MD::TileSize = 0;

		$MD::TileStart = getRealTime();

		MD_makeTileRequest("/tile_metadata");
	}
	else if($MD::TileCurrentYIndex <= $MD::TileMaxYIndex) {
		MD_makeTileRequest("/tile", $MD::TileCurrentXIndex, $MD::TileCurrentYIndex);
		$MD::TileCurrentXIndex++;

		// wrap numbers around
		if($MD::TileCurrentXIndex > $MD::TileMaxXIndex) {
			$MD::TileCurrentXIndex = 0;
			$MD::TileCurrentYIndex++;
		}
	}
	else {
		echo("MD_TileDownloader: finished entire map in" SPC mFloor((getRealTime() - $MD::TileStart) / 1000) SPC "seconds for" SPC ($MD::TileMaxXIndex * $MD::TileMaxYIndex) SPC "tiles");
	}
}

function MD_makeTileRequest(%endpoint, %xQuery, %yQuery) {
	%tcp = new TCPObject(MD_TileDownloader);
	
	if(%xQuery !$= "" && %yQuery !$= "") {
		%endpoint = %endpoint @ "?x=" @ %xQuery @ "&y=" @ %yQuery;
		%tcp.file = "tile_" @ %xQuery @ "_" @ %yQuery @ ".png";
	}
	
	%tcp.connect("45.32.215.224:80");
	%tcp.endpoint = %endpoint;
	%tcp.request = "GET /md" @ %endpoint SPC "HTTP/1.1\r\nHost: 45.32.215.224\r\nAccept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9\r\n\r\n";
	%tcp.schedule(30000, delete); // we shouldn't be keeping these connections open for that long
	return %tcp;
}

function MD_TileDownloader::onConnected(%this) {
	%this.send(%this.request);
}

function MD_TileDownloader::onLine(%this, %line) {
	if(%this.endpoint $= "/tile_metadata") {
		if(%line $= "") {
			%this.expectingData = true;
		}
		else if(%this.expectingData) {
			$MD::TileMaxXIndex = getWord(%line, 0);
			$MD::TileMaxYIndex = getWord(%line, 1);
			$MD::TileXResolution = getWord(%line, 2);
			$MD::TileYResolution = getWord(%line, 3);
			$MD::TileSize = getWord(%line, 4);

			%this.schedule(10, delete);
			MD_downloadTile(); // download tiles now
		}
	}
	else if(strPos(strLwr(%line), "content-type:") != -1 && strPos(strLwr(%line), "image/png") != -1) {
		%this.length = getWord(%line, 1);
		%this.downloadFile = true;
	}
		
	if(%line $= "" && %this.downloadFile) {
		%this.setBinarySize(%this.length);
	}
}

function MD_TileDownloader::onBinChunk(%this, %chunk) {
	if(%chunk >= %this.len) {
		%this.saveBufferToFile("config/client/MiniDungeons/tiles/" @ %this.file);
		%this.schedule(10, delete);
		echo("MD_TileDownloader: saved" SPC %this.file);
		schedule(33, 0, MD_downloadTile); // download the next tile
	}
}