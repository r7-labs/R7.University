# R7.University

[![Build Status](https://travis-ci.org/roman-yagodin/R7.University.svg?branch=master)](https://travis-ci.org/roman-yagodin/R7.University)
[![Coverity Scan Build Status](https://scan.coverity.com/projects/7326/badge.svg)](https://scan.coverity.com/projects/roman-yagodin-r7-university)
[![Join the chat at https://gitter.im/roman-yagodin/R7.University](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/roman-yagodin/R7.University?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Modules and base library for DNN Platfrom CMS designed to present and manage various assets 
(e.g. divisions, employees, educational programs, documents) for high school educational organization website.

## Dependencies

- DNN Platform 7.4.2
- [R7.ImageHandler](https://github.com/roman-yagodin/R7.ImageHandler) to generate thumbnails. Must be installed separately.
- [YamlDotNet](https://github.com/aaubry/YamlDotNet) to read config files. Included in the install package.

## Example of use

*R7.University* modules actively used on the official website of *Volgograd State Agricultural University* http://www.volgau.com. 
On [this page](http://www.volgau.com/LinkClick.aspx?link=284) you could see *Employee*, *EmployeeList* and *Division* module instances. 

## Short TODO

- [ ] DNN ContentItems API integration
- [ ] Support http://schema.org formats
- [ ] Allow manage multiple organizations
- [x] Add portal-level configuration options
