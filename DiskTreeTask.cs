using System;
using System.Collections.Generic;
using System.Linq;

namespace DiskTree {
    // класс, представляющий папку в дереве диска
    public class Folder {
        // имя текущего каталога
        public string Name { get; }
        // подпапки текущей папки
        public Dictionary<string, Folder> Subfolders { get; }

        // конструктор класса
        public Folder(string name) {
            Name = name;
            Subfolders = new Dictionary<string, Folder>();
        }

        // метод для получения подпапки или добавления новой, если её нет
        public Folder GetOrAddSubfolder(string subfolderName) {
            // попытка получить подпапку с заданным именем
            if (Subfolders.TryGetValue(subfolderName, out var subfolder)) {
                return subfolder;
            }
            // подпапка с заданным именем не найдена, создаем новую подпапку
            else {
                subfolder = new Folder(subfolderName);
                // добавляем новую подпапку в коллекцию подпапок текущей папки
                Subfolders[subfolderName] = subfolder;
                return subfolder;
            }
        }

        // метод для получения отформатированного дерева
        public List<string> GetFormattedTree(int depth, List<string> result) {
            // уровень вложенности, для исключения корневой папки из отступов
            if (depth != -1) {
                result.Add(new string(' ', depth) + Name);
            }

            // рекурсивный вызов метода для каждой подпапки, отсортированной по имени
            foreach (var subfolder in GetSortedSubfolders()) {
                subfolder.GetFormattedTree(depth + 1, result);
            }

            // список отформатированных строк
            return result;
        }

        // вспомогательный метод для получения подкаталогов в отсортированном порядке
        private IEnumerable<Folder> GetSortedSubfolders() {
            return Subfolders.Values.OrderBy(subfolder => subfolder.Name, StringComparer.Ordinal);
        }
    }

    // статический класс для решения задачи по построению дерева каталогов
    public static class DiskTreeTask {
        // метод для построения дерева каталогов и получения отформатированного списка строк
        public static List<string> Solve(List<string> directoryPaths) {
            // создание корневой папки с пустым именем
            var rootFolder = new Folder("");

            // обработка каждого пути входного списка
            foreach (var path in directoryPaths) {
                // вызов метода для обработки пути и построения структуры папок
                ProcessPath(rootFolder, path);
            }

            // получение отформатированного дерева в виде списка строк
            return rootFolder.GetFormattedTree(-1, new List<string>());
        }

        // вспомогательный метод для обработки пути и построения соответствующей структуры в дереве
        private static void ProcessPath(Folder currentFolder, string fullPath) {
            // разделение пути на подкаталоги
            var subfolderNames = fullPath.Split('\\');
            foreach (var subfolderName in subfolderNames) {
                // получение или добавление подкаталога к текущему каталогу
                currentFolder = currentFolder.GetOrAddSubfolder(subfolderName);
            }
        }
    }
}
