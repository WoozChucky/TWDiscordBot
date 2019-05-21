﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace TWDiscordBot.Commands.Preconditions
{
    public class RequireRoleAttribute : PreconditionAttribute
    {
        private readonly string _name;

        public RequireRoleAttribute(string name) => _name = name;

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            // Check if this user is a Guild User, which is the only context where roles exist
            if (!(context.User is SocketGuildUser gUser))
                return Task.FromResult(PreconditionResult.FromError("You must be in a guild to run this command."));
            
            // If this command was executed by a user with the appropriate role, return a success
            if (gUser.Roles.Any(r => r.Name == _name))
                // Since no async work is done, the result has to be wrapped with `Task.FromResult` to avoid compiler errors
                return Task.FromResult(PreconditionResult.FromSuccess());
                
            // Since it wasn't, fail
            return Task.FromResult(PreconditionResult.FromError($"You must have a role named {_name} to run this command."));

        }
    }
}
