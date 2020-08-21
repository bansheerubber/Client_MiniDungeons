if(!isObject(MD_Tutorial)) {
	exec("./guis/MD_Tutorial.gui");
}

function clientCmdMD_showTutorial(%gui) {
	if(isObject(%gui)) {
		showMiniDungeonsTutorial(%gui);
	}
}

function showMiniDungeonsTutorial(%gui) {
	if(!isObject(MiniDungeonsTutorialSound)) {
		datablock AudioProfile(MiniDungeonsTutorialSound) {
			filename    = "Add-Ons/Client_MiniDungeons/sounds/tutorial open.ogg";
			description = Audio2d;
			preload = true;
		};
	}

	%count = MD_Tutorial.getCount();
	for(%i = 0; %i < %count; %i++) {
		MD_Tutorial.getObject(%i).visible = false;
	}
	
	alxPlay(MiniDungeonsTutorialSound);
	%gui.setCentered();
	%gui.visible = true;

	if(%gui.hasPages) {
		for(%i = 1; %i <= %gui.pageCount; %i++) {
			%index = 0;
			while((%name = %gui.pageName[%index]) !$= "") {
				(%gui.pageName[%index] @ %i).visible = %i == 1;
				%index++;
			}
		}
		%gui.currentPage = 1;
		%gui.pageButton.setText(%gui.pageButton.originalText);
	}

	Canvas.pushDialog(MD_Tutorial);
}

function miniDungeonsAdvanceTutorialPage(%gui) {
	alxPlay(MiniDungeonsTutorialSound);
	
	if(%gui.currentPage >= %gui.pageCount) {
		Canvas.popDialog(MD_Tutorial);
		return;
	}

	%gui.currentPage++;
	%page = %gui.currentPage;
	for(%i = 1; %i <= %gui.pageCount; %i++) {
		%index = 0;
		while((%name = %gui.pageName[%index]) !$= "") {
			(%gui.pageName[%index] @ %i).visible = %i == %page;
			%index++;
		}
	}

	if(%page == %gui.pageCount) { // next to last page
		%gui.pageButton.setText(%gui.pageButton.finalText);
	}
}