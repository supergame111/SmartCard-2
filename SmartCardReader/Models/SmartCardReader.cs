using Net.Sf.Pkcs11;
using Net.Sf.Pkcs11.Objects;
using Net.Sf.Pkcs11.Wrapper;
using SmartCard.Interfaces;
using SmartCardReader.DAL;
using SmartCardReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCard.Models
{
    //implementatie volgens repo beID
    public class BeIDSmartCard: ISmartCard
    {
        //todo: Get photo file (of general files)
        //todo: check 'GetData'

        //private Module _module;
        private string _dllname;
        public BeIDSmartCard(string filename)
        {
            _dllname = filename;
        }
        public BeIDSmartCard()
        {
            //Wat doet deze dll / waar gevonden?
            //_module = Module.GetInstance("beidpkcs11.dll");
            _dllname = "beidpkcs11.dll";
        }

        public bool IsSlotActive(int id)
        {
            using (Module m = Module.GetInstance(_dllname))
            {
                Slot[] slots = m.GetSlotList(true);
  
                return slots[id].SlotInfo.IsTokenPresent;
            }
        }
        
        public bool HasActiveSlots()
        {
            using (Module m = Module.GetInstance(_dllname))
            {
                Slot[] slots = m.GetSlotList(true);
                return slots.Length > 0;
            }
        }

        public string GetSlotDescription(int id)
        {
            //initialization now occurs within the getinstance function
            //m.Initialize();
            using (Module m = Module.GetInstance(_dllname))
            {
                // Look for slots (cardreaders)
                // GetSlotList(false) will return all cardreaders
                Slot[] slots = m.GetSlotList(false);
                if (slots.Length == 0)
                    return "";
                else
                    return slots[id].SlotInfo.SlotDescription.Trim();
            }
        }

        public void PullData(out IdentityCard candidate)
        {

            candidate = new IdentityCard {
                Surname = GetData("surname"),
                Firstnames = GetData("firstnames"),
                Nationality = GetData("nationality"),
                BirthPlace = GetData("location_of_birth"),
                BirthDay = GetData("date_of_birth"),
                Gender = GetData("gender"),
                Nobility = GetData("nobility"),
                CardNumber = GetData("card_number"),
                Photohash = GetData("photo_hash"),
                //HasFamily = bool.Parse(GetData("member_of_family")),
                Address = GetData("address_street_and_number"),
                City = GetData("address_municipality"),
                ZipCode=GetData("address_zip")
            };
            // candidate = new CandidateGrid
            //{
            //    City = GetData("location_of_birth"),
            //    Name = GetData("firstname") + GetData("surname"),
            //    Email= "Default",
            //    PrimaryPhone="Default",
                


            //};
        
        }

        public SlotInfo GetSlotInfo(int slotid)
        {
            using (Module m = Module.GetInstance(_dllname))
            {
                Slot[] slots = m.GetSlotList(false);
                return slots[slotid].SlotInfo;
            }
        }

        public Slot[] Slots()
        {
            using(Module m = Module.GetInstance(_dllname))
            {
                Slot[] slots = m.GetSlotList(false);
                return slots;
            }
        }
        public string GetTokenInfoLabel()
        {
            using(Module m = Module.GetInstance(_dllname))
            {
                return m.GetSlotList(true)[0].Token.TokenInfo.Label.Trim();
            }
        }

        private string GetData(string label)
        {
 
            using (Module m = Module.GetInstance(_dllname))
            {
                string value = null;
                Data data;

                // Get the first slot (cardreader) with a token
                Slot[] slotlist = m.GetSlotList(true);
                if (slotlist.Length > 0)
                {
                    Slot slot = slotlist[0];

                    Session session = slot.Token.OpenSession(true);

                    // Search for objects
                    // First, define a search template 

                    // "The label attribute of the objects should equal ..."
                    ByteArrayAttribute classAttribute = new ByteArrayAttribute(CKA.CLASS);
                    classAttribute.Value = BitConverter.GetBytes((uint)Net.Sf.Pkcs11.Wrapper.CKO.DATA);


                    ByteArrayAttribute labelAttribute = new ByteArrayAttribute(CKA.LABEL);
                    labelAttribute.Value = System.Text.Encoding.UTF8.GetBytes(label);


                    session.FindObjectsInit(new P11Attribute[] { classAttribute, labelAttribute });
                    P11Object[] foundObjects = session.FindObjects(50);
                    int counter = foundObjects.Length;
                    while (counter > 0)
                    {
                        //foundObjects[counter-1].ReadAttributes(session);
                        //public static BooleanAttribute ReadAttribute(Session session, uint hObj, BooleanAttribute attr)
                        data = foundObjects[counter - 1] as Data;
                        label = data.Label.ToString();
                        if (label != null)
                            Console.WriteLine(label);
                        if (data.Value.Value != null)
                        {
                            value = System.Text.Encoding.UTF8.GetString(data.Value.Value);
                            Console.WriteLine(value);
                        }
                        counter--;
                    }
                    session.FindObjectsFinal();
                    session.Dispose();
                }
                else
                {
                    throw new Exception("Card not found");
                    //Console.WriteLine("No card found\n");
                }
                return value;
            }
            
        }

        /// <summary>
        /// Get surname of the owner of the token (eid) in the first non-empty slot (cardreader)
        /// </summary>
        /// <returns></returns>
        public string GetSpecialStatus()
        {
            return GetData("special_status");
        }

        /// <summary>
        /// Get surname of the owner of the token (eid) in the first non-empty slot (cardreader)
        /// </summary>
        /// <returns></returns>
        public string GetSurname()
        {
            return GetData("surname");
        }

        /// <summary>
        /// Get date of birth of the owner. This is a language specific string
        /// More info about the format can be found in the eid specs.
        /// </summary>
        /// <returns></returns>
        public string GetDateOfBirth()
        {
            return GetData("date_of_birth");
        }
    }
}