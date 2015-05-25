using System;
using System.Collections.Generic;
using InventoryQuest.Components.Items;

namespace InventoryQuest.Components.Entities.Player
{
    [Serializable]
    public class PasiveSkills
    {
        private readonly List<long> _Experience = new List<long>();
        private readonly List<int> _Level = new List<int>();

        public PasiveSkills()
        {
            for (var i = 0; i < Enum.GetValues(typeof (EnumItemClassSkill)).Length; i++)
            {
                _Experience.Add(0);
                _Level.Add(1);
            }
        }

        /// <summary>
        ///     Return skill level
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public long GetSkillLevelByEnum(EnumItemClassSkill skill)
        {
            return _Level[(int) skill];
        }

        /// <summary>
        ///     Return skill experience
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public long GetSkillExperienceByEnum(EnumItemClassSkill skill)
        {
            return _Experience[(int) skill];
        }

        /// <summary>
        ///     Add experience to skill
        /// </summary>
        /// <param name="skill">Skill</param>
        /// <param name="addExperience">Experience to add</param>
        public void AddSkillExperienceByEnum(EnumItemClassSkill skill, long addExperience)
        {
            _Experience[(int) skill] += addExperience;
            tryPromote(skill);
        }

        /// <summary>
        ///     Return experience required to next level
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public long GetToNextLevelExperience(EnumItemClassSkill skill)
        {
            return GetSkillLevelByEnum(skill)*GetSkillLevelByEnum(skill)*1230;
        }

        /// <summary>
        ///     Try to promote skill
        /// </summary>
        /// <param name="skill"></param>
        private void tryPromote(EnumItemClassSkill skill)
        {
            var expected = GetToNextLevelExperience(skill);
            if (GetSkillExperienceByEnum(skill) >= expected)
            {
                _Experience[(int) skill] = GetSkillExperienceByEnum(skill) - expected;
                _Level[(int) skill]++;
            }
        }
    }
}