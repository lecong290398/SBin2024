﻿using System;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using DTKH2024.SbinSolution.Friendships.Dto;

namespace DTKH2024.SbinSolution.Chat.Dto
{
    public class GetUserChatFriendsWithSettingsOutput
    {
        public DateTime ServerTime { get; set; }
        
        public List<FriendDto> Friends { get; set; }

        public GetUserChatFriendsWithSettingsOutput()
        {
            Friends = new EditableList<FriendDto>();
        }
    }
}