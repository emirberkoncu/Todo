using System;
using System.Collections.Generic;
using System.Linq;

namespace todolist
{
    // Kart Büyüklüğü Enum
    enum Size { XS = 1, S, M, L, XL }

    // Takım Üyesi sınıfı
    class TeamMember
    {
        public int Id { get; }
        public string Name { get; }

        public TeamMember(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    // Kart Sınıfı
    class Card
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public TeamMember AssignedMember { get; set; }
        public Size CardSize { get; set; }

        public Card(string title, string content, TeamMember assignedMember, Size cardSize)
        {
            Title = title;
            Content = content;
            AssignedMember = assignedMember;
            CardSize = cardSize;
        }
    }

    // Board sınıfı
    class Board
    {
        public Dictionary<string, List<Card>> Lines { get; } = new Dictionary<string, List<Card>>()
        {
            { "TODO", new List<Card>() },
            { "IN PROGRESS", new List<Card>() },
            { "DONE", new List<Card>() }
        };
    }

    class Program
    {
        static List<TeamMember> teamMembers = new List<TeamMember>
        {
            new TeamMember(1, "Alice"),
            new TeamMember(2, "Bob"),
            new TeamMember(3, "Charlie")
        };

        static Board board = new Board();

        static void Main(string[] args)
        {
            // Varsayılan Kartlar
            board.Lines["TODO"].Add(new Card("Görev 1", "İçerik 1", teamMembers[0], Size.S));
            board.Lines["IN PROGRESS"].Add(new Card("Görev 2", "İçerik 2", teamMembers[1], Size.M));
            board.Lines["DONE"].Add(new Card("Görev 3", "İçerik 3", teamMembers[2], Size.L));

            while (true)
            {
                Console.WriteLine("Lütfen yapmak istediğiniz işlemi seçiniz :) ");
                Console.WriteLine("*******************************************");
                Console.WriteLine("(1) Board Listelemek");
                Console.WriteLine("(2) Board'a Kart Eklemek");
                Console.WriteLine("(3) Board'dan Kart Silmek");
                Console.WriteLine("(4) Kart Taşımak");
                Console.WriteLine("(5) Çıkış");
                Console.Write("Seçiminiz: ");
                
                switch (Console.ReadLine())
                {
                    case "1": ListBoard(); break;
                    case "2": AddCard(); break;
                    case "3": RemoveCard(); break;
                    case "4": MoveCard(); break;
                    case "5": return;
                    default: Console.WriteLine("Geçersiz seçim!"); break;
                }
            }
        }

        static void ListBoard()
        {
            foreach (var line in board.Lines)
            {
                Console.WriteLine($"{line.Key} Line\n************************");

                if (!line.Value.Any())
                {
                    Console.WriteLine("~ BOŞ ~\n");
                    continue;
                }

                foreach (var card in line.Value)
                {
                    Console.WriteLine($"Başlık      : {card.Title}");
                    Console.WriteLine($"İçerik      : {card.Content}");
                    Console.WriteLine($"Atanan Kişi : {card.AssignedMember.Name}");
                    Console.WriteLine($"Büyüklük    : {card.CardSize}\n-");
                }
            }
        }

        static void AddCard()
        {
            Console.Write("Başlık Giriniz: ");
            string title = Console.ReadLine();

            Console.Write("İçerik Giriniz: ");
            string content = Console.ReadLine();

            Console.Write("Büyüklük Seçiniz -> XS(1),S(2),M(3),L(4),XL(5): ");
            if (!Enum.TryParse(Console.ReadLine(), out Size cardSize))
            {
                Console.WriteLine("Geçersiz büyüklük seçimi!");
                return;
            }

            Console.WriteLine("Kişi Seçiniz: ");
            foreach (var member in teamMembers)
                Console.WriteLine($"{member.Id} - {member.Name}");

            if (!int.TryParse(Console.ReadLine(), out int memberId) || !teamMembers.Any(m => m.Id == memberId))
            {
                Console.WriteLine("Hatalı girişler yaptınız!");
                return;
            }

            TeamMember assignedMember = teamMembers.First(m => m.Id == memberId);
            board.Lines["TODO"].Add(new Card(title, content, assignedMember, cardSize));
        }

        static void RemoveCard()
        {
            Console.Write("Lütfen silmek istediğiniz kart başlığını yazınız: ");
            string title = Console.ReadLine();

            foreach (var line in board.Lines)
            {
                var card = line.Value.FirstOrDefault(c => c.Title == title);
                if (card != null)
                {
                    line.Value.Remove(card);
                    Console.WriteLine("Kart başarıyla silindi.");
                    return;
                }
            }

            Console.WriteLine("Aradığınız kriterlere uygun kart board'da bulunamadı.");
        }

        static void MoveCard()
        {
            Console.Write("Lütfen taşımak istediğiniz kart başlığını yazınız: ");
            string title = Console.ReadLine();

            foreach (var line in board.Lines)
            {
                var card = line.Value.FirstOrDefault(c => c.Title == title);
                if (card != null)
                {
                    Console.WriteLine("Bulunan Kart Bilgileri:");
                    Console.WriteLine($"Başlık      : {card.Title}");
                    Console.WriteLine($"İçerik      : {card.Content}");
                    Console.WriteLine($"Atanan Kişi : {card.AssignedMember.Name}");
                    Console.WriteLine($"Büyüklük    : {card.CardSize}");
                    Console.WriteLine($"Line        : {line.Key}");

                    Console.Write("Lütfen taşımak istediğiniz Line'ı seçiniz: (1) TODO (2) IN PROGRESS (3) DONE: ");
                    string choice = Console.ReadLine();

                    string targetLine = choice switch
                    {
                        "1" => "TODO",
                        "2" => "IN PROGRESS",
                        "3" => "DONE",
                        _ => null
                    };

                    if (targetLine == null || targetLine == line.Key)
                    {
                        Console.WriteLine("Hatalı bir seçim yaptınız!");
                        return;
                    }

                    line.Value.Remove(card);
                    board.Lines[targetLine].Add(card);
                    Console.WriteLine("Kart başarıyla taşındı.");
                    return;
                }
            }

            Console.WriteLine("Aradığınız kriterlere uygun kart board'da bulunamadı.");
        }
    }
}
