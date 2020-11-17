//
//  SystemDocumentType.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2019 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace R7.University
{
    /// <summary>
    /// Types of documents, recognized by the system to do type-specific processing
    /// </summary>
    public enum SystemDocumentType
    {
        // ru-RU: образовательный стандарт
        EduStandard,
        // ru-RU: профессиональный стандарт
        ProfStandard,
        // ru-RU: образовательная программа
        EduProgram,
        // ru-RU: календарный график
        EduSchedule,
        // ru-RU: учебный план
        EduPlan,
        // ru-RU: методический материал
        EduMaterial,
        // ru-RU: аннотации рабочей программы
        WorkProgramAnnotation,
        // ru-RU: рабочая программа практики
        WorkProgramOfPractice,
        // ru-RU: рабочая программа
        WorkProgram,
        Custom
    }
}

