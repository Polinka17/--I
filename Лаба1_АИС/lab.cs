using BusinesLogical;

internal class lab
{
    static void Main(string[] args)
    {
        Logic logic = new Logic();

        while (true)
        {
            Console.WriteLine("1 — Добавить студента");
            Console.WriteLine("2 — Изменить студента");
            Console.WriteLine("3 — Удалить студента");
            Console.WriteLine("4 — Показать всех");
            Console.WriteLine("5 — Группировка по специальностям");
            Console.WriteLine("6 — Поиск студентов по группе");
            Console.WriteLine("0 — Выход");
            Console.Write("Выберите действие: ");

            string cmd = Console.ReadLine();

            if (cmd == "0")
                break;

            switch (cmd)
            {
                case "1":
                    Console.Write("Имя: ");
                    string name = Console.ReadLine();
                    Console.Write("Группа: ");
                    string group = Console.ReadLine();
                    Console.Write("Специальность: ");
                    string spec = Console.ReadLine();
                    logic.AddStudent(name, group, spec);
                    break;

                case "2":
                    Console.Write("Кого изменить (имя): ");
                    string oldName = Console.ReadLine();
                    Console.Write("Новое имя: ");
                    string newName = Console.ReadLine();
                    Console.Write("Новая группа: ");
                    string newGroup = Console.ReadLine();
                    Console.Write("Новая специальность: ");
                    string newSpec = Console.ReadLine();

                    if (logic.UpdateStudent(oldName, newName, newGroup, newSpec))
                        Console.WriteLine("Готово!");
                    else
                        Console.WriteLine("Студент не найден!");
                    break;

                case "3":
                    Console.Write("Кого удалить (имя): ");
                    string delName = Console.ReadLine();
                    if (logic.DeleteStudent(delName))
                        Console.WriteLine("Удалён.");
                    else
                        Console.WriteLine("Студент не найден!");
                    break;

                case "4":
                    Console.WriteLine("Список студентов:");
                    foreach (var s in logic.GetAll())
                        Console.WriteLine(s);
                    break;

                case "5":
                    Console.WriteLine("Группировка по специальностям:");
                    var groups = logic.GroupBySpeciality();
                    foreach (var g in groups)
                    {
                        Console.WriteLine($"\nСпециальность: {g.Key}");
                        foreach (var s in g.Value)
                            Console.WriteLine($"  {s.Name} ({s.Group})");
                    }
                    break;

                case "6":
                    Console.Write("Введите группу: ");
                    string gname = Console.ReadLine();
                    var found = logic.GetStudentsByGroup(gname);

                    if (found.Count == 0)
                        Console.WriteLine("Никого не найдено.");
                    else
                    {
                        Console.WriteLine("Студенты:");
                        foreach (var s in found)
                            Console.WriteLine($"{s.Name} {s.Speciality}");
                    }
                    break;

                default:
                    Console.WriteLine("Неверная команда!");
                    break;
            }

            Console.WriteLine();
        }
    }
}
