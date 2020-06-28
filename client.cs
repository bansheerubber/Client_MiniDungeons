schedule(1000, 0, exec, "Add-Ons/Client_MiniDungeons/adjustment.cs");

exec("./profiles/outline.cs");

if(!isObject(MD_Healthbar)) {
	exec("./guis/MD_Healthbar.gui");
}
exec("./healthbar.cs");
exec("./parry.cs");
exec("./welcome.cs");

$MDC::Version = "r1";

function clientCmdMD_Handshake() {
	commandToServer('MD_Handshake_Ack', $MDC::Version);

	PlayGUI.add(MD_Healthbar);
	MD_Healthbar.hp = 100;
	MD_Healthbar.maxHp = 100;
	MD_Healthbar.resizeHealthbar();
}