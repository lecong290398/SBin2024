using System.Collections.Generic;

namespace DTKH2024.SbinSolution.Chat.Dto
{
    public class ChatUserWithMessagesDto : ChatUserDto
    {
        public List<ChatMessageDto> Messages { get; set; }
    }
}