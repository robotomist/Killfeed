using Backtrace.Unity.Extensions;
using Bloodstone.API;
using HarmonyLib;
using ProjectM;
using ProjectM.Network;
using ProjectM.Gameplay.Systems;
using Unity.Collections;
using Unity.Entities;

namespace Killfeed;

[HarmonyPatch(typeof(CreateGameplayEventsOnDamageTakenSystem), nameof(CreateGameplayEventsOnDamageTakenSystem.CreateEventOnDamageTaken_Execute))]
public static class TakenDamageHook
{
    public static void P(string msg)
    {
        ServerChatUtils.SendSystemMessageToAllClients(VWorld.Server.EntityManager, msg);
    }
    public static void Prefix(CreateGameplayEventsOnDamageTakenSystem __instance)
    {
        NativeArray<Entity> entities = __instance.__query_1365518740_0.ToEntityArray(Allocator.Temp);
        NativeArray<Entity> damageTakenEventQEntities = __instance._DamageTakenEventQuery.ToEntityArray(Allocator.Temp);
        P($"LL {damageTakenEventQEntities.Length}");
        foreach (var ent in damageTakenEventQEntities)
        {
            if (ent.Has<EntityOwner>())
            {
                P("EntityOwner");
                var entOwner = ent.Read<EntityOwner>();
                var myOwner = entOwner.Owner;
                if (myOwner.Has<PlayerCharacter>())
                {
                    var ownerPlayer = myOwner.Read<PlayerCharacter>();
                    P($"IT HAS IT {ownerPlayer.Name}");
                }
                else
                {
                    P("IT DOES NOT HAVE IT");
                }

            }
            if (ent.Has<DamageTakenEvent>())
            {
                P("DamageTakenEvent");
            }
        }

        // foreach (var entity in entities)
        // {
        //     if (entity.Has<SpellTarget>())
        //     {
        //         P("THAT'S THE ONE)");
        //     }
        //     if (entity.Has<DamageTakenEvent>())
        //     {
        //         P("HANDLEDAMGETAKEN");
        //         HandleDamageTakenEvent(entity);
        //     }
        //     else
        //     {
        //         P("Nope");
        //     }
        // }
    }
    private static void HandleDamageTakenEvent(Entity entity)
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
}
