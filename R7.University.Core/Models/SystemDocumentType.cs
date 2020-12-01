namespace R7.University.Models
{
    /// <summary>
    /// Types of documents, recognized by the system to do type-specific processing
    /// </summary>
    public enum SystemDocumentType
    {
        // ru: федеральный государственный образовательный стандарт
        StateEduStandard,
        // ru: образовательный стандарт
        EduStandard,
        // ru: профессиональный стандарт
        ProfStandard,
        // ru: образовательная программа
        EduProgram,
        // ru: календарный график
        EduSchedule,
        // ru: учебный план
        EduPlan,
        // ru: методический материал
        EduMaterial,
        // ru: аннотации рабочей программы
        WorkProgramAnnotation,
        // ru: рабочая программа практики
        WorkProgramOfPractice,
        // ru: рабочая программа
        WorkProgram,
        Custom
    }
}

