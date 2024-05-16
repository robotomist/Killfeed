using Backtrace.Unity.Extensions;
using Bloodstone.API;
using HarmonyLib;
using ProjectM;
using ProjectM.Behaviours;
using ProjectM.Gameplay.Systems;
using Unity.Collections;
using Unity.Entities;

namespace Killfeed;

[HarmonyPatch(typeof(CreateGameplayEventsOnDamageTakenSystem), nameof(CreateGameplayEventsOnDamageTakenSystem.CreateEventOnDamageTaken_Execute))]
public static class DealDamageHook
{
    public static void P(string msg)
    {
        ServerChatUtils.SendSystemMessageToAllClients(VWorld.Server.EntityManager, msg);
    }
    public static void Prefix(CreateGameplayEventsOnDamageTakenSystem __instance)
    {
        NativeArray<Entity> entities = __instance.__query_1365518740_0.ToEntityArray(Allocator.Temp);
        foreach (var entity in entities)
        {
            if (entity.Has<DamageTakenEvent>())
            {
                DamageTakenEvent damageTakenEvent = entity.Read<DamageTakenEvent>();
                if (damageTakenEvent.Entity.Has<PlayerCharacter>())
                {
                    PlayerCharacter damageTakenPlayerCharacter = damageTakenEvent.Entity.Read<PlayerCharacter>();
                    P($"{damageTakenPlayerCharacter.Name} has take damage from: ???");

                }
                else
                {
                    P("A non player was hit");
                }
            }
            else
            {
                P("Nope");
            }
        }
    }
}
