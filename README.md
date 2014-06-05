#R7.University

Modules and base library for DNN Platfrom CMS designed to present and manage various assets 
(divisions, employees) for high school education website. 


**NOTE**: From version 1.0.4 R7.University uses [R7.ImageHandler](https://github.com/roman-yagodin/R7.ImageHandler) 
to generate photo thumbnails, and from 1.0.6 - for QR-code generation of contact vCards (but not absolutely require it). Url format for thumbnails is http://www.somewhere.com/imagehandler.ashx?fileid=XXX&width=YYY, so you can also use / adapt [BBImageHandler] (http://bbimagehandler.codeplex.com/) or others image generators (but prepare that some things won't work, like barcodes with non-latin content, etc.)

#Example of use

R7.University modules currently used on the official website of Volgograd State Agricultural University www.volgau.com. On [this page](http://www.volgau.com/%D1%83%D0%B8%D0%BA%D1%82) you could see Employee, EmployeeList and Division module instances. 

#TODO

* Support schema.org formats
* Allow export contacts in vCard format
* Allow manage multiple organizations
* Add organization-level configuration options
* Add more modules
* Many, many other things
