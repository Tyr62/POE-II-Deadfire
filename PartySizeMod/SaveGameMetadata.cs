using System;
using System.IO;
using Game;
using Onyx;
using Patchwork;
using UnityEngine;

namespace PoE2Mods.PartySizeMod
{
    [ModifiesType]
    public class mod_SaveGameMetadata : SaveGameMetadata
    {
       [ModifiesMember("SavePartyPortraits")]
        private void SavePartyPortraitsNew()
        {
            mPartyPortraits = (Texture2D[])new Texture2D[6];
            PartyPortraitsRawData = new byte[6][];

            int num = 0;
            foreach (PartyMember activePrimaryPartyMember in SingletonBehavior<PartyManager>.Instance.GetActivePrimaryPartyMembers())
            {
                if (!(activePrimaryPartyMember == null))
                {
                    try
                    {
                        string path = FileUtility.CombinePath(SaveLoadUtils.WorkingSaveGamePath, "partyMember" + num.ToStringInvariant() + ".png");
                        var val = Portrait.GetTexture(activePrimaryPartyMember, Portrait.Style.Small);
                        if (val != null)
                        {
                            Texture2D val2 = GameUtilities.ResizeTexture((Texture2D)val, 32, 41);
                            byte[] array = val2.EncodeToPNG();
                            PartyPortraitsRawData[num++] = array;
                            File.WriteAllBytes(path, array);
                            ResourceManager.DestroyTexture(val2);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogException(ex);
                        Debug.LogError((object)("Error saving '" + activePrimaryPartyMember.name + "' portrait."));
                    }
                }
            }
        }
    }
}
