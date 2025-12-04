using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Model;
using System.Xml.Linq;

namespace BusinesLogical
{
    public class Logic
    {
        List<Student> Students = new List<Student>();
        public void AddStudent(string name, string group, string speciality)
        {
            Student student = new Student()
            {
                Name = name,
                Group = group,
                Speciality = speciality,
            };
            Students.Add(student);

        }
        public List<Student> GetAll()
        {
            return Students.ToList();
        }
        public bool UpdateStudent(string name, string newName, string newGroup, string newSpeciality)
        {
            Student student = Students.FirstOrDefault(s => s.Name == name);
            if (student == null)
            {
                return false;
            }

            student.Name = newName;
            student.Group = newGroup;
            student.Speciality = newSpeciality;

            return true;
        }
        public bool DeleteStudent(string name)
        {
            Student student = Students.FirstOrDefault(s => s.Name == name);
            if (student == null)
            {
                return false; 
            }

            Students.Remove(student);
            return true;
        }
 
        public Dictionary<string, List<Student>> GroupBySpeciality()
        {
            return Students
                .GroupBy(s => s.Speciality)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public List<Student> GetStudentsByGroup(string group)
        {
            return Students
                .Where(s => s.Group.Equals(group, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }


    }
}