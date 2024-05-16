namespace Domain.Entities.CoursAssistes;

public enum Note
{
    Incomplet,
    A,
    B,
    C,
    D,
    Echec
}

public static class NoteExtensions
{
    public static byte ToByte(this Note note)
    {
        return note switch
        {
            Note.Incomplet => 50,
            Note.A => 95,
            Note.B => 85,
            Note.C => 75,
            Note.D => 65,
            Note.Echec => 55,
            _ => throw new ArgumentOutOfRangeException(nameof(note), note, null)
        };
    }
}