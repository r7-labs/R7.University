//
// SystemDocumentType.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;

namespace R7.University
{
    /// <summary>
    /// Types of documents, recognized by the system to do type-specific processing
    /// </summary>
    public enum SystemDocumentType
    {
        OrderEnrollment,       // ru-RU: приказ о приеме (зачислении)
        OrderExpulsion,        // ru-RU: приказ об отчислении
        OrderRestoration,      // ru-RU: приказ о восстановлении
        OrderTransfer,         // ru-RU: приказ о переводе
        OrderAcademicLeave,    // ru-RU: приказ об академическом отпуске
        EduStandard,           // ru-RU: образовательный стандарт
        EduProgram,            // ru-RU: образовательная программа
        EduSchedule,           // ru-RU: календарный график
        EduPlan,               // ru-RU: учебный план
        EduMaterial,           // ru-RU: методический материал
        WorkProgramAnnotation, // ru-RU: аннотация (аннотации) рабочей программы
        WorkProgramOfPractice, // ru-RU: рабочая программа практики
        Contingent,            // ru-RU: сведения о численности обучающихся
        ContingentMovement,    // ru-RU: сведения о результатах перевода, восстановления и отчисления
        ScienceInfo,           // ru-RU: сведения о направлениях и результатах НИР
        Custom
    }
}

