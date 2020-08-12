if(!isObject(MD_Potion)) {
	exec("./guis/MD_Potion.gui");
}

if(!isObject(MD_PotionTransfer)) {
	exec("./guis/MD_PotionTransfer.gui");
}

function MD_PotionTransfer::setBars(%this, %barrelHP, %barrelMaxHP, %potionHP, %potionMaxHP) {
	MD_Potion.hideUI();
	
	%this.setCentered();

	%height = mFloor(MD_PotionTransfer1.maxHeight * (%barrelHP / %barrelMaxHP));
	MD_PotionTransfer1.resize(
		getWord(MD_PotionTransfer1.position, 0),
		getWord(MD_PotionTransferBackground1.position, 1) + getWord(MD_PotionTransferBackground1.extent, 1) - %height - MD_PotionTransfer1.border,
		getWord(MD_PotionTransfer1.extent, 0),
		%height
	);

	%height = mFloor(MD_PotionTransfer2.maxHeight * (%potionHP / %potionMaxHP));
	MD_PotionTransfer2.resize(
		getWord(MD_PotionTransfer2.position, 0),
		getWord(MD_PotionTransferBackground2.position, 1) + getWord(MD_PotionTransferBackground2.extent, 1) - %height - MD_PotionTransfer2.border,
		getWord(MD_PotionTransfer2.extent, 0),
		%height
	);
}

function MD_PotionTransfer::showUI(%this) {
	PlayGUI.add(MD_PotionTransfer);
}

function MD_PotionTransfer::hideUI(%this) {
	if(PlayGUI.isMember(MD_PotionTransfer)) {
		PlayGUI.remove(MD_PotionTransfer);
	}
}

function clientCmdMD_SetPotionTransferBar(%barrelHP, %barrelMaxHP, %potionHP, %potionMaxHP) {
	MD_PotionTransfer.showUI();
	MD_PotionTransfer.setBars(%barrelHP, %barrelMaxHP, %potionHP, %potionMaxHP);
}

function MD_Potion::setBar(%this, %sipAmount, %gulpAmount, %remainingHP, %maxHP) {
	%this.setCentered();
	
	MD_PotionBackground.resize(
		getWord(MD_PotionBackground.position, 0),
		getWord(MD_PotionBackground.position, 1),
		mFloor(MD_PotionBackground.maxWidth * (%remainingHP / %maxHP)),
		getWord(MD_PotionBackground.extent, 1)
	);

	%active = "<color:FFFF00>";
	%disabled = "<color:555555>";
	MD_PotionText.setText(formatText(
		MD_PotionText.format,
		%sipAmount <= %remainingHP ? %active : %disabled,
		%sipAmount,
		%active,
		%gulpAmount <= %remainingHP ? %active : %disabled,
		%gulpAmount
	));
}

function MD_Potion::showUI(%this) {
	PlayGUI.add(MD_Potion);
}

function MD_Potion::hideUI(%this) {
	if(PlayGUI.isMember(MD_Potion)) {
		PlayGUI.remove(MD_Potion);
	}
}

function clientCmdMD_SetPotionBar(%sipAmount, %gulpAmount, %remainingHP, %maxHP) {
	MD_Potion.showUI();
	MD_Potion.setBar(%sipAmount, %gulpAmount, %remainingHP, %maxHP);
}

function clientCmdMD_HidePotionBar() {
	MD_Potion.hideUI();
	MD_PotionTransfer.hideUI();
}

deActivatePackage(MiniDungeonsClientPotion);
package MiniDungeonsClientPotion {
	function disconnect() {
		Parent::disconnect();

		MD_Potion.hideUI();
	}
};
activatePackage(MiniDungeonsClientPotion);