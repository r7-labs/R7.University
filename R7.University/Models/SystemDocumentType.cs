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

