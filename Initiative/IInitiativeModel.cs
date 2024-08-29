namespace Initiative;

public interface IInitiativeModel
{
    Task<InitiativeResult> Reset(ChannelId channel);
    Task<InitiativeResult> Next(ChannelId channel, bool skipToEnd);
    Task<InitiativeRollResult> Roll(ChannelId channel, string roll, string name, bool hidden, PingId? ping, UserId user);
    Task<InitiativeRollResult> Modify(ChannelId channel, string name, string roll);
    Task<InitiativeResult> Check(ChannelId channel);
    Task<InitiativeResult> Delete(ChannelId channel, string name);
    Task<InitiativeRollResult> Reroll(ChannelId channel, UserId user);
    Task<InitiativeResult> Unhide(ChannelId channel, string name);
    Task<ResultEnum> SetServerInitiativeType(ServerId server, InitiativeConfiguration config);
}
