using System.ComponentModel;

namespace TarefasBackEnd.Enums
{
    public enum StatusTarefa
    {
        [Description("A fazer")]
        AFazer = 0,
        [Description("Em Andamento")]
        EmAndamento = 1,
        [Description("Concluída")]
        Concluida = 2
    }
}