using System;
using System.Collections.Generic;
using System.Linq;
using R7.University.Models;

namespace R7.University.ModelExtensions
{
    public static class DocumentExtensions
    {
        public static SystemDocumentType GetSystemDocumentType (this IDocument document)
        {
            SystemDocumentType result;
            return Enum.TryParse<SystemDocumentType> (document.DocumentType.Type, out result) ? result : SystemDocumentType.Custom;
        }

        public static void SetModelId (this IDocumentWritable document, ModelType modelType, int modelId)
        {
            if (modelType == ModelType.EduProgram) {
                document.EduProgramId = modelId;
            }
            else if (modelType == ModelType.EduProgramProfile) {
                document.EduProgramProfileId = modelId;
            }
            else {
                throw new ArgumentException ($"Wrong modelType={modelType} argument.");
            }
        }

        public static IEnumerable<IDocument> OrderByGroupDescThenSortIndex (this IEnumerable<IDocument> documents)
        {
            return documents.OrderByDescending (d => d.Group, DocumentGroupComparer.Instance).ThenBy (d => d.SortIndex).ThenBy (d => d.Title);
        }
    }
}

