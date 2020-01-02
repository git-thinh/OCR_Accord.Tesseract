tesseractdotnet based on tesseract-orc v3.01 r590

- added wrappers (refer to folder: wrapper)

- added "tessedit_thresholding_method" in tesseract-ocr (refer to: wrapper\lathr.h)

- sample: refer to tesseractconsole project.

Usage threshold mode:

	- call SetVariable() method before passing image (api->SetImage(image))
	// 0: otsu
        // 1: isodata local adaptive
        // 2: sauvola local adaptive => not implement yet
        //processor.SetVariable("tessedit_thresholding_method", "0");
	processor.SetVariable("tessedit_thresholding_method", "1");
	
////////////////////////////////////////////////////////////////////////////////

https://code.google.com/archive/p/tesseractdotnet/


////////////////////////////////////////////////////////////////////////////////

tesseractdotnet_tesseract3.0.1_IPoVn


https://code.google.com/archive/p/tesseractdotnet/downloads
https://github.com/charlesw/tesseract-ocr-dotnet
 
Project Information
The project was created on Feb 17, 2011.

License: Apache License 2.0
69 stars
svn-based source control
Labels:
tesseract-ocr tesseractdotnet imageprocessing


tesseract-ocr .net

Introduction
This is tesseract-ocr .net project.
Objectives
Tesseract Engine Wrapper:
Features
  * The current version (1.0RC) supported:
    1. get/set variables environment,
    1. analyse page layout,
    1. extract locations of detected characters,
    1. be able to run parallel for a set of images.
Sources
  * Refer to [here](https://tesseractdotnet.googlecode.com/svn/trunk/dotnetwrapper/TesseractEngineWrapper)
Documents
  * Refer to [Tutorial](http://code.google.com/p/tesseractdotnet/wiki/TesseractEngineWrapper)
Tesseract Engine-based .net
Features
Sources
Documents
Simple example application
Link

Screenshots

////////////////////////////////////////////////////////////////////////////////


////////////////////////////////////////////////////////////////////////////////

