# R7.University

[![Build Status](https://travis-ci.org/roman-yagodin/R7.University.svg?branch=master)](https://travis-ci.org/roman-yagodin/R7.University)
[![BCH compliance](https://bettercodehub.com/edge/badge/roman-yagodin/R7.University)](https://bettercodehub.com/)
[![Coverity Scan Build Status](https://scan.coverity.com/projects/7326/badge.svg)](https://scan.coverity.com/projects/roman-yagodin-r7-university)
[![Join the chat at https://gitter.im/roman-yagodin/R7.University](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/roman-yagodin/R7.University?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Modules and base library for DNN Platfrom designed to present and manage various assets 
(e.g. divisions, employees, educational programs, documents) for high school educational organization website,
compatible with [obrnadzor.gov.ru](http://obrnadzor.gov.ru) requirements on information structure and microdata.

## На русском

Модули расширения и базовая библиотека для DNN Platform, предназначенные для отображения и управления информацией
о различных ресурсах образовательной организации (подразделениях, сотрудниках, образовательных программах, документах и т.д.)
на сайте образовательной организации высшего образования, совместимые с рекомендациями [Рособрнадзора](http://obrnadzor.gov.ru)
относительно структуры информации и микроразметки.

## License

[![AGPLv3](https://www.gnu.org/graphics/agplv3-155x51.png)](https://www.gnu.org/licenses/agpl-3.0.html)

The *R7.University* is free software: you can redistribute it and/or modify it under the terms of 
the GNU Affero General Public License as published by the Free Software Foundation, either version 3 of the License, 
or (at your option) any later version.

## Dependencies

- [DNN Platform v8.0.4](https://github.com/dnnsoftware/Dnn.Platform)
- [R7.Dnn.Extensions](https://github.com/roman-yagodin/R7.Dnn.Extensions) as base library. Must be installed separately.
- [R7.ImageHandler](https://github.com/roman-yagodin/R7.ImageHandler) to generate thumbnails. Must be installed separately.
- [Entity Framework](https://github.com/aspnet/EntityFramework6) to work with database. Currently included in the install package.

## Example of use

See *R7.University* modules in action on the official website of *Volgograd State Agricultural University*:

- [Employee, EmployeeList and Division modules](http://www.volgau.com/LinkClick.aspx?link=284)
- [EduProgram module](http://www.volgau.com/LinkClick.aspx?link=7276)
- [DivisionDirectory modules](http://www.volgau.com/sveden/struct)
- [EmployeeDirectory module](http://www.volgau.com/sveden/employees)
- [EduProgramProfileDirectory modules](http://www.volgau.com/sveden/education)
- [EduProgramDirectory modules](http://www.volgau.com/sveden/edustandarts)
- [ContingentDirectory module](http://www.volgau.com/sveden/education/chislen)
- [ScienceDirectory module](http://www.volgau.com/sveden/education/nir)
