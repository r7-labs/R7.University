using System;
using System.Collections.Generic;
using System.Linq;
using R7.University.Models;

namespace R7.University.ModelExtensions
{
    public static class DivisionExtensions
    {
        public static IEnumerable<TDivision> CalculateLevelAndPath<TDivision> (this IEnumerable<TDivision> divisions)
            where TDivision : IDivisionWritable
        {
            var rootDivisions = divisions.Where (d => d.ParentDivisionID == null);
            foreach (var root in rootDivisions) {
                CalculateLevelAndPath (root, -1, string.Empty);
            }

            return divisions;
        }

        private static void CalculateLevelAndPath<TDivision> (TDivision division, int level, string path)
            where TDivision : IDivisionWritable
        {
            division.Level = level + 1;
            division.Path = path + "/" + division.DivisionID.ToString ().PadLeft (10, '0');

            if (division.SubDivisions != null) {
                foreach (var subDivision in division.SubDivisions) {
                    CalculateLevelAndPath (subDivision, division.Level, division.Path);
                }
            }
        }

        public static void SetModelId (this IEduProgramDivisionWritable division, ModelType modelType, int modelId)
        {
            if (modelType == ModelType.EduProgram) {
                division.EduProgramId = modelId;
            }
            else if (modelType == ModelType.EduProgramProfile) {
                division.EduProgramProfileId = modelId;
            }
            else {
                throw new ArgumentException ($"Wrong modelType={modelType} argument.");
            }
        }

        public static IEnumerable<IOccupiedPosition> GetHeadEmployeePositions (this IDivision division)
        {
            return division.OccupiedPositions.Where (op => op.PositionID == division.HeadPositionID);
        }
    }
}
