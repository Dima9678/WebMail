using Domain.Models.Requests;

namespace Server.Normaizators
{
    public static class InputNormalizator
    {
        public static RegisterDTO NormalizeRegisterDTO(RegisterDTO dto)
        {
            dto.Name = dto.Name.Trim();
            dto.Email = dto.Email.Trim();
            dto.Email = dto.Email.ToLower();
            dto.Password = dto.Password.Trim();
            dto.RepeatPassword = dto.Password.Trim();

            dto.Email += "@mymail.com";

            return dto;
        }
        public static LoginDTO NormalizeLoginDTO(LoginDTO dto)
        {
            dto.Email = dto.Email.Trim();
            dto.Email = dto.Email.ToLower();
            dto.Password = dto.Password.Trim();

            return dto;
        }
        public static string[] SplitEmails(string emailString)
        {
            string[] rawEmailsArray = emailString.Split(" ");
            List<string> endEmailsList = new List<string>();

            for (int i = 0; i < rawEmailsArray.Length; i++)
            {
                if (rawEmailsArray[i].Contains("@mymail.com"))
                {
                    if (rawEmailsArray[i].EndsWith("@mymail.com"))
                    {
                        endEmailsList.Add(rawEmailsArray[i]);
                    }
                    else
                    {
                        string a = rawEmailsArray[i];
                        while (!a.EndsWith("@mymail.com"))
                        {
                            a = a[0..(a.Length - 1)];
                        }
                        endEmailsList.Add(a);
                    }

                }
            }
            List<string> emails = new List<string>();
            for (int i = 0; i < endEmailsList.Count; i++)
            {
                var indexes = ContainsCount(endEmailsList[i]);
                if (indexes.Count > 1)
                {
                    emails.Add(endEmailsList[i][0..(indexes[0] + 11)]);
                    for (int j = 0; j < indexes.Count - 1; j++)
                    {
                        int skippedSymbols = 0;

                        for (int k = 0; k < endEmailsList[i].Length; k++)
                        {
                            char a = endEmailsList[i][indexes[j] + 11 + k];
                            if (endEmailsList[i][indexes[j] + 11 + k] == ',')
                            {
                                skippedSymbols++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        emails.Add(endEmailsList[i][(indexes[j] + 11 + skippedSymbols)..(indexes[j + 1] + 11)]);
                    }
                    endEmailsList.RemoveAt(i);
                    endEmailsList.AddRange(emails);
                }
            }
            return endEmailsList.ToArray();
        }
    
    private static List<int> ContainsCount(string str)
        {
            List<int> indexes = new List<int>();
            for (int i = 0; i < str.Length - 10; i++)
            {
                if (str[i..(i + 11)] == "@mymail.com")
                {
                    indexes.Add(i);
                }
            }
            return indexes;
        }
    }
}