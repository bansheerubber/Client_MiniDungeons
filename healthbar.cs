if(!isObject(MD_Healthbar)) {
	exec("./guis/MD_Healthbar.gui");
}

$MDC::HealthbarScaleFactor = 3000;

function MD_Healthbar::resizeHealthbar(%this, %testRes) {
	%resolution = %testRes !$= "" ? %testRes : getRes();
	%width = getWord(%resolution, 0);
	%height = getWord(%resolution, 1);
	%scaleFactor = %width / $MDC::HealthbarScaleFactor;

	MD_Healthbar.extent = %width SPC %height;
	
	%newExtent1 = %this.getHealthbarExtent(%testRes); // used for sword bitmap, healthbar bitmap, and healthbar parent
	%newExtent2 = %this.getMaskExtent(%testRes); // sword mask
	%position = %this.getParentPosition(%testRes);

	MD_Healthbar_Parent.resize(getWord(%position, 0), getWord(%position, 1), getWord(%newExtent1, 0), getWord(%newExtent1, 1));
	MD_Healthbar_Sword.resize(0, 0, getWord(%newExtent1, 0), getWord(%newExtent1, 1));
	MD_Healthbar_Bar.resize(0, 0, getWord(%newExtent1, 0), getWord(%newExtent1, 1));
	MD_Healthbar_Mask.resize(0, 0, getWord(%newExtent2, 0), getWord(%newExtent2, 1));

	// healthbar text
	%newTextPosition = vectorScale("150 64", %scaleFactor);
	%newTextExtent = vectorScale("405 45", %scaleFactor);
	MD_Healthbar_Text.resize(getWord(%newTextPosition, 0), getWord(%newTextPosition, 1), getWord(%newTextExtent, 0), getWord(%newTextExtent, 1));
	MD_Healthbar_Text.fontSize = 45 * %scaleFactor;
	MD_Healthbar_Text.setText("<font:Palatino Linotype:" @ MD_Healthbar_Text.fontSize @ "><color:dddddd>" @ MD_Healthbar.hp @ "hp");

	MD_Healthbar.setHealthbar(MD_Healthbar.hp / MD_Healthbar.maxHp, %testRes);

	// guard cycle text
	%newGuardPosition = vectorScale("165 5", %scaleFactor);
	%newGuardExtent = vectorScale("405 45", %scaleFactor);
	MD_Guard_Text.resize(getWord(%newGuardPosition, 0), getWord(%newGuardPosition, 1), getWord(%newGuardExtent, 0), getWord(%newGuardExtent, 1));
	MD_Guard_Text.fontSize = 45 * %scaleFactor;
	MD_Guard_Text.setText("<font:Palatino Linotype:" @ MD_Guard_Text.fontSize @ "><color:dddddd>M -> <color:ffff33>H<color:dddddd> -> M");

	MD_Guard_Text.setActiveCycle(MD_Guard_Text.activeCycle);
}

function MD_Healthbar::getParentPosition(%this, %testRes) {
	%resolution = %testRes !$= "" ? %testRes : getRes();
	%width = getWord(%resolution, 0);
	%height = getWord(%resolution, 1);
	
	%extent = %this.getHealthbarExtent(%testRes);
	return 10 SPC %height - getWord(%extent, 1) - 10;
}

function MD_Healthbar::getMaskParams(%this, %testRes) {
	%resolution = %testRes !$= "" ? %testRes : getRes();
	%width = getWord(%resolution, 0);
	%height = getWord(%resolution, 1);
	%scaleFactor = %width / $MDC::HealthbarScaleFactor;

	%startAndEnd = "883 132";
	return getWords(vectorScale(%startAndEnd, %scaleFactor), 0, 1);
}

function MD_Healthbar::getMaskExtent(%this, %testRes) {
	%extent = "883 177";
	%resolution = %testRes !$= "" ? %testRes : getRes();
	%width = getWord(%resolution, 0);
	%height = getWord(%resolution, 1);
	%scaleFactor = %width / $MDC::HealthbarScaleFactor;

	return getWords(vectorScale(%extent, %scaleFactor), 0, 1);
}

function MD_Healthbar::getHealthbarExtent(%this, %testRes) {
	%extent = "908 177";
	%resolution = %testRes !$= "" ? %testRes : getRes();
	%width = getWord(%resolution, 0);
	%height = getWord(%resolution, 1);
	%scaleFactor = %width / $MDC::HealthbarScaleFactor;

	return getWords(vectorScale(%extent, %scaleFactor), 0, 1);
}

function MD_Healthbar::setHealthbar(%this, %percent, %testRes) {
	%startAndEnd = %this.getMaskParams(%testRes);
	%lerp = getWord(%startAndEnd, 1) + (getWord(%startAndEnd, 0) - getWord(%startAndEnd, 1)) * %percent;
	MD_Healthbar_Mask.resize(0, 0, mFloor(%lerp), getWord(%this.getMaskExtent(%testRes), 1));
}

function MD_Healthbar::vibrateHealthbar(%this, %ticks, %shake) {
	cancel(%this.vibrateHealthbar);

	if(%ticks > 0) {
		%position = %this.getParentPosition();
		%shakeX = getWord(%shake, 0);
		%shakeY = getWord(%shake, 1);
		MD_Healthbar_Parent.position = getWords(vectorAdd(%position, getRandom(-%shakeX, %shakeX) SPC getRandom(-%shakeY, %shakeY)), 0, 1);
		
		%this.vibrateHealthbar = %this.schedule(16, vibrateHealthbar, %ticks - 1, %shake);

		MD_Healthbar_Sword.mColor = "255 75 75 255";
	}
	else {
		MD_Healthbar_Parent.position = %this.getParentPosition();
		MD_Healthbar_Sword.mColor = "255 255 255 255";
	}
}

function clientCmdMD_SetHealthbar(%hp, %maxHp) {
	MD_Healthbar.setHealthbar(%hp / %maxHp);

	if(%hp >= 1) {
		%hp = mFloor(%hp);
	}
	else if(%hp > 0.1) {
		%hp = mFloatLength(%hp, 1);
	}
	else { // for those special moments when you have 0.05hp left to spare
		%hp = mFloatLength(%hp, 2);
	}
	MD_Healthbar_Text.setText("<font:Palatino Linotype:" @ MD_Healthbar_Text.fontSize @ "><color:dddddd>" @ %hp @ "hp");

	MD_Healthbar_Text.hp = %hp;
	MD_Healthbar_Text.maxHp = %maxHp;
}

function clientCmdMD_VibrateHealthbar(%ticks, %shake) {
	MD_Healthbar.vibrateHealthbar(%ticks, %shake);
}

function clientCmdMD_LoadGuardCycles(%cycle0, %cycle1, %cycle2, %cycle3, %cycle4, %cycle5) {
	%count = 6;
	for(%i = 0; %i < %count; %i++) {
		MD_Guard_Text.guard[%i] = %cycle[%i];
	}
	MD_Guard_Text.setActiveCycle(MD_Guard_Text.activeCycle | 0);
}

function clientCmdMD_SetActiveCycle(%index) {
	MD_Guard_Text.setActiveCycle(%index);
}

function MD_Guard_Text::setActiveCycle(%this, %index) {
	// reduce guards into string
	if(%this.guard[0] $= "") {
		MD_Guard_Text.setText("");
	}
	else {
		%output = "<font:Palatino Linotype:" @ %this.fontSize @ "><color:dddddd>";
		%count = 6;
		for(%i = 0; ((%cycle = %this.guard[%i]) !$= ""); %i++) { // keep looping until we hit empty cycle
			if(%i == %index) {
				%output = %output @ "<color:ffff33>" @ %cycle @ "<color:dddddd> -> ";
			}
			else {
				%output = %output @ %cycle @ " -> ";
			}
		}
		
		%output = trim(getSubStr(%output, 0, strLen(%output) - 4));
		MD_Guard_Text.setText(%output);

		MD_Guard_Text.activeCycle = %index;
	}
}

deActivatePackage(MiniDungeonsClientHealthbar);
package MiniDungeonsClientHealthbar {
	function PlayGUI::onRender(%this) {
		Parent::onRender(%this);

		if(MD_Healthbar.ready) {
			MD_Healthbar.resizeHealthbar();
		}
	}

	function disconnect() {
		Parent::disconnect();
		
		if(MD_Healthbar.ready) {
			PlayGUI.remove(MD_Healthbar);
			MD_Healthbar.ready = false;
		}
	}
};
activatePackage(MiniDungeonsClientHealthbar);