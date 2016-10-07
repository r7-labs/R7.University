# R7.University

[![Build Status](https://travis-ci.org/roman-yagodin/R7.University.svg?branch=master)](https://travis-ci.org/roman-yagodin/R7.University)
[![Coverity Scan Build Status](https://scan.coverity.com/projects/7326/badge.svg)](https://scan.coverity.com/projects/roman-yagodin-r7-university)
[![Join the chat at https://gitter.im/roman-yagodin/R7.University](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/roman-yagodin/R7.University?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Modules and base library for DNN Platfrom designed to present and manage various assets 
(e.g. divisions, employees, educational programs, documents) for high school educational organization website,
compatible with [obrnadzor.gov.ru](http://obrnadzor.gov.ru/microformats) microdata requirements.

## License

[![AGPLv3](https://www.gnu.org/graphics/agplv3-155x51.png)](https://www.gnu.org/licenses/agpl-3.0.html)

The *R7.University* is free software: you can redistribute it and/or modify it under the terms of 
the GNU Affero General Public License as published by the Free Software Foundation, either version 3 of the License, 
or (at your option) any later version.

## Dependencies

- DNN Platform 7.4.2
- [R7.DotNetNuke.Extensions](https://github.com/roman-yagodin/DotNetNuke.Extensions) as base library. Must be installed separately.
- [R7.ImageHandler](https://github.com/roman-yagodin/R7.ImageHandler) to generate thumbnails. Must be installed separately.
- [YamlDotNet](https://github.com/aaubry/YamlDotNet) to read config files. Currently included in the install package.

## Example of use

See *R7.University* modules in action on the official website of *Volgograd State Agricultural University*:

- [Employee, EmployeeList and Division modules](http://www.volgau.com/LinkClick.aspx?link=284)
- [DivisionDirectory module](http://www.volgau.com/sveden/struct)
- [EmployeeDirectory module](http://www.volgau.com/sveden/employees)
- [EduProgramProfileDirectory modules](http://www.volgau.com/sveden/education)
- [EduProgramDirectory modules](http://www.volgau.com/sveden/edustandarts)
- [EduProgram module](http://www.volgau.com/LinkClick.aspx?link=7276)

## Short TODO

- [ ] DNN ContentItems API integration
- [ ] Support http://schema.org formats
- [ ] Allow manage multiple organizations
- [x] Add portal-level configuration options

