//
//  SystemDocumentType.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
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

using System;

namespace R7.University
{
    /// <summary>
    /// Types of documents, recognized by the system to do type-specific processing
    /// </summary>
    public enum SystemDocumentType
    {
        // ru-RU: приказ о приеме (зачислении)
        OrderEnrollment,
        // ru-RU: приказ об отчислении
        OrderExpulsion,
        // ru-RU: приказ о восстановлении
        OrderRestoration,
        // ru-RU: приказ о переводе
        OrderTransfer,
        // ru-RU: приказ об академическом отпуске
        OrderAcademicLeave,
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
        // ru-RU: аннотация (аннотации) рабочей программы
        WorkProgramAnnotation,
        // ru-RU: рабочая программа практики
        WorkProgramOfPractice,
        // ru-RU: сведения о численности обучающихся
        Contingent,
        // ru-RU: сведения о результатах перевода, восстановления и отчисления
        ContingentMovement,
        // ru-RU: сведения о направлениях и результатах НИР
        ScienceInfo,
        Custom
    }
}

