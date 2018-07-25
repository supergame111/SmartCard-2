using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.HighLevelAPI;
using SmartCard.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCardReader.Models
{
    public class SmartCardReaderInterop: ISmartCard
    {
        private static string[] labels = {
            "card_number",
            "validity_begin_date",
            "validity_end_date",
            "surname",
            "firstnames",
            "nationality",
            "location_of_birth",
            "date_of_birth",
            "gender",
            "nobility",
            "photo_hash",
            "address_street_and_number",
            "address_zip",
            "address_municipality",
            };
        private string dllname;
        public SmartCardReaderInterop()
        {
            this.dllname = "beidpkcs11.dll";
        }
        public SmartCardReaderInterop(string dllname)
        {
            this.dllname = dllname;
        }
        
        public void PullData(out IdentityCard card)
        {
            try
            {
                Dictionary<string, string> data = GetData(labels);
                card = new IdentityCard
                {
                    Surname = data["surname"],
                    Firstnames = data["firstnames"],
                    Nationality = data["nationality"],
                    BirthPlace = data["location_of_birth"],
                    BirthDay = data["date_of_birth"],
                    Gender = data["gender"],
                    Nobility = data["nobility"],
                    CardNumber = data["card_number"],
                    Photohash = data["photo_hash"],
                    //HasFamily = bool.Parse(GetData("member_of_family")),
                    Address = data["address_street_and_number"],
                    City = data["address_municipality"],
                    ZipCode = data["address_zip"]
                };
            }
            catch (KeyNotFoundException e)
            {
                var edata = e.Data;
                throw;
            }
            

        }

        private Dictionary<string, string> GetData(string[] labels)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            using (Pkcs11 card = new Pkcs11(dllname, AppType.SingleThreaded))
            {
                card.WaitForSlotEvent(WaitType.Blocking, out bool e, out ulong slotid);
                foreach (var slot in card.GetSlotList(SlotsType.WithTokenPresent))
                {

                    Session session = slot.OpenSession(SessionType.ReadOnly);
                    foreach (string label in labels)
                    {
                        List<ObjectHandle> handles = session.FindAllObjects(new List<ObjectAttribute> { new ObjectAttribute(CKA.CKA_CLASS, CKO.CKO_DATA), new ObjectAttribute(CKA.CKA_LABEL, label) });
                        foreach (var handle in handles)
                        {
                            List<ObjectAttribute> attr = session.GetAttributeValue(handle, new List<CKA> { CKA.CKA_VALUE });
                            string value = attr.First().GetValueAsString();
                            if (string.IsNullOrEmpty(value))
                            {
                                value = "Nothing found";
                            }
                            data.Add(label, value);
                        }
                    }
                }
            }
            return data;
        }


        public bool IsSlotActive(int id)
        {
            using (Pkcs11 card = new Pkcs11(dllname, AppType.SingleThreaded))
            {
                List<Slot> slots = card.GetSlotList(SlotsType.WithTokenPresent);
                return slots.ElementAt(id).GetSlotInfo().SlotFlags.TokenPresent;
            }
        }
    }
}