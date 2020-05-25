function GuiEditorResList::onSelect(%this, %id) {
	switch(%id)
	{
		case 640:
			GuiEditorRegion.resize(0,0,640,480);
			GuiEditorContent.getObject(0).resize(0,0,640,480);
		case 800:
			GuiEditorRegion.resize(0,0,800,600);
			GuiEditorContent.getObject(0).resize(0,0,800,600);
		case 1024:
			GuiEditorRegion.resize(0,0,1024,768);
			GuiEditorContent.getObject(0).resize(0,0,1024,768);
		case 1280:
			GuiEditorRegion.resize(0,0,1280,720);
			GuiEditorContent.getObject(0).resize(0,0,1280,720);
		case 1920:
			GuiEditorRegion.resize(0,0,1920,1080);
			GuiEditorContent.getObject(0).resize(0,0,1920,1080);
	}
}

if($MDC::Adjustment $= "") {
	GuiEditorResList.add("1280 x 720", 1280);
	GuiEditorResList.add("1920 x 1080", 1920);
	
	$MDC::Adjustment = true;
}