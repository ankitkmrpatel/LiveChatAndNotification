using NotificationBackendService.Data;
using NotificationBackendService.Data.Entities;
using System;
using System.Collections;
using System.Collections.Generic;

namespace NotificationBackendService;

public class PrepData
{
    public static void PrepPopulation(IApplicationBuilder app)
    {
        using var serviceScoped = app.ApplicationServices.CreateScope();
        using var dbContext = serviceScoped.ServiceProvider.GetService<AppDataContext>();
        SeedData(dbContext);
    }

    private static void SeedData(AppDataContext? dbContext)
    {
        if (dbContext != null)
        {
            dbContext.Database.EnsureCreated();
            Console.WriteLine("Seeding Data");

            if (!dbContext.Organizations.Any())
            {
                dbContext.Organizations.AddRange(GetOrganizations());
                dbContext.SaveChanges();
            }

            if (!dbContext.Shipments.Any())
            {
                dbContext.Shipments.AddRange(GetShipments());
                dbContext.SaveChanges();
            }

            if (!dbContext.ShipmentMilestones.Any())
            {
                var shipments = dbContext.Shipments.ToList();
                if (shipments.Any())
                {
                    //Add Milestone
                    foreach (var shipment in shipments)
                    {
                        var shipmentMilestone = GetShipmentMIlestones(shipment.ShipmentId);
                        dbContext.ShipmentMilestones.AddRange(shipmentMilestone);
                    }

                    dbContext.SaveChanges();
                }
            }

            if (!dbContext.Users.Any())
            {
                dbContext.Users.AddRange(GetUsers());
                dbContext.SaveChanges();
            }

            if (!dbContext.Events.Any())
            {
                dbContext.Events.AddRange(GetEvents());
                dbContext.SaveChanges();
            }

            if (!dbContext.UserEventSubsSettings.Any())
            {
                var usrs = dbContext.Users.ToList();
                if (usrs.Any())
                {
                    foreach (var usr in usrs)
                    {
                        var settings = GetUserSubsSettings(usr);
                        dbContext.UserEventSubsSettings.AddRange(settings);

                        var shipments = dbContext.Shipments.Where(x => (!string.IsNullOrEmpty(x.CnorCode) && x.CnorCode == usr.Organizations)
                            || (!string.IsNullOrEmpty(x.CneeCode) && x.CneeCode == usr.Organizations)
                            || (!string.IsNullOrEmpty(x.NotifCode) && x.NotifCode == usr.Organizations))
                            .ToList();

                        if (shipments == null)
                        {
                            continue;
                        }

                        foreach (var shipment in shipments)
                        {
                            var customSettings = GetCustomUserSubsSettings(usr, shipment.ShipmentId);
                            dbContext.UserEventSubsSettings.AddRange(customSettings);
                        }
                    }

                    dbContext.SaveChanges();
                }
            }

            Console.WriteLine("Seeded Data");
        }
    }

    private static UserEventSubsSetting[] GetUserSubsSettings(User usr)
    {
        return new UserEventSubsSetting[]
        {
            new UserEventSubsSetting()
            {
                UserId = usr.Id,
                EventModuleType = "SHP",
                EventModuleId = "*",
                SubsSettingsItems = new List<UserEventSubsSettingsItem>()
                {
                    new UserEventSubsSettingsItem()
                    {
                        EventType = "MILESTONE",
                        EventItem = "Follow up on Telex HBL",
                        NotificationTimeout = TimeSpan.FromMinutes(15),
                        EventSubsDeliveryMode = "N",
                    },
                    new UserEventSubsSettingsItem()
                    {
                        EventType = "MILESTONE",
                        EventItem = "Add Pre Alert to eDocs",
                        NotificationTimeout = TimeSpan.FromMinutes(15),
                        EventSubsDeliveryMode = "N",
                    },
                    new UserEventSubsSettingsItem()
                    {
                        EventType = "MILESTONE",
                        EventItem = "Send Cartage Advice to Transporter",
                        NotificationTimeout = TimeSpan.FromMinutes(15),
                        EventSubsDeliveryMode = "N",
                    },
                    new UserEventSubsSettingsItem()
                    {
                        EventType = "MILESTONE",
                        EventItem = "Delivered",
                        NotificationTimeout = TimeSpan.FromMinutes(15),
                        EventSubsDeliveryMode = "N",
                    },
                    ////new UserEventSubsSettingsItem()
                    ////{
                    ////    EventType = "MILESTONE",
                    ////    EventItem = "*",
                    ////    NotificationTimeout = TimeSpan.FromMinutes(5),
                    ////    EventSubsDeliveryMode = "N",
                    ////}
                }
            }
        };
    }

    private static UserEventSubsSetting[] GetCustomUserSubsSettings(User usr, string shipmentId)
    {
        var shipmentMilestones = GetShipmentMIlestones(shipmentId);
        var listToReturn = new List<ShipmentMilestone>();

        var deck = CreateShuffledDeck(shipmentMilestones);

        for (var i = 0; i < 5; i++)
        {
            var arrayItem = deck.Pop();
            if (arrayItem == null) continue;
            listToReturn.Add((ShipmentMilestone)arrayItem);
        }

        return new UserEventSubsSetting[]
        {
            new UserEventSubsSetting()
            {
                UserId = usr.Id,
                EventModuleType = "SHP",
                EventModuleId = shipmentId,
                SubsSettingsItems = listToReturn.Select(x => new UserEventSubsSettingsItem()
                {
                    EventType = "MILESTONE",
                    EventItem = x.Description,
                    NotificationTimeout = TimeSpan.FromMinutes(15),
                    EventSubsDeliveryMode = "N",
                })
                .ToList()
            }
        };
    }
    public static Stack CreateShuffledDeck(IEnumerable<ShipmentMilestone> values)
    {

        var random = new Random();
        var list = new List<ShipmentMilestone>(values);
        var stack = new Stack();

        while (list.Count > 0)
        {
            // Get the next item at random.
            var randomIndex = random.Next(0, list.Count);
            var randomItem = list[randomIndex];
            // Remove the item from the list and push it to the top of the deck.
            list.RemoveAt(randomIndex);
            stack.Push(randomItem);
        }

        return stack;
    }

    private static IEnumerable<Organization> GetOrganizations()
    {
        return new Organization[]
        {
            new Organization()
            {
                Name = "Test Name 1",
                Code = "TST1",
                Phone = "+91-9891123456",
                Email = "ankit11@sflhu.com"
            },
            new Organization()
            {
                Name = "Test Name 2",
                Code = "TST2",
                Phone = "+91-9891123456",
                Email = "ankit11@sflhu.com"
            },
            new Organization()
            {
                Name = "Test Name 3",
                Code = "TST3",
                Phone = "+91-9891123456",
                Email = "ankit11@sflhu.com"
            },
            new Organization()
            {
                Name = "Test Name 4",
                Code = "TST4",
                Phone = "+91-9891123456",
                Email = "ankit11@sflhu.com"
            },
            new Organization()
            {
                Name = "Test Name 5",
                Code = "TST5",
                Phone = "+91-9891123456",
                Email = "ankit11@sflhu.com",
            },
        };
    }

    private static IEnumerable<Shipment> GetShipments()
    {
        return new Shipment[]
        {
            new Shipment()
            {
                ShipmentId = "S00001234",
                CneeCode = "TST1",
                CneeName = "Test Name 1",
                CnorCode = "TST3",
                CnorName = "Test Name 3",
                GoodsDesc = "Shipment Goods Desc S00001234",
                POLCode = "USNYC",
                POLName = "New York",
                PODCode = "INDEL",
                PODName = "New Delhi"
            },
            new Shipment()
            {
                ShipmentId = "S00001345",
                CneeCode = "TST2",
                CneeName = "Test Name 2",
                CnorCode = "TST3",
                CnorName = "Test Name 3",
                GoodsDesc = "Shipment Goods Desc S00001345",
                POLCode = "USNYC",
                POLName = "New York",
                PODCode = "INDEL",
                PODName = "New Delhi"
            },
            new Shipment()
            {
                ShipmentId = "S00001554",
                CneeCode = "TST5",
                CneeName = "Test Name 5",
                CnorCode = "TST4",
                CnorName = "Test Name 4",
                GoodsDesc = "Shipment Goods Desc S00001554",
                POLCode = "USNYC",
                POLName = "New York",
                PODCode = "INDEL",
                PODName = "New Delhi"
            },
            new Shipment()
            {
                ShipmentId = "S00001664",
                CneeCode = "TST2",
                CneeName = "Test Name 2",
                CnorCode = "TST4",
                CnorName = "Test Name 4",
                GoodsDesc = "Shipment Goods Desc S00001664",
                POLCode = "USNYC",
                POLName = "New York",
                PODCode = "INDEL",
                PODName = "New Delhi"
            },
            new Shipment()
            {
                ShipmentId = "S00001744",
                CneeCode = "TST1",
                CneeName = "Test Name 1",
                CnorCode = "TST5",
                CnorName = "Test Name 5",
                GoodsDesc = "Shipment Goods Desc S00001234",
                POLCode = "USNYC",
                POLName = "New York",
                PODCode = "INDEL",
                PODName = "New Delhi"
            },
            new Shipment()
            {
                ShipmentId = "S00001894",
                CneeCode = "TST3",
                CneeName = "Test Name 3",
                NotifCode = "TST5",
                NotifName = "TST5",
                CnorCode = "TST1",
                CnorName = "Test Name 1",
                GoodsDesc = "Shipment Goods Desc S00001234",
                POLCode = "USNYC",
                POLName = "New York",
                PODCode = "INDEL",
                PODName = "New Delhi"
            },
        };
    }

    private static IEnumerable<ShipmentMilestone> GetShipmentMIlestones(string shipment)
    {
        return new ShipmentMilestone[]
        {
            new ShipmentMilestone()
            {
                Sequence = 1,
                Description = "Register Shipment on CW",
                EventCode = "JOP",
                ActualDate = DateTime.Now.ToString(),
                EstimatedDate = DateTime.Now.ToString(),
                ShipmentId = shipment,
            },
            new ShipmentMilestone()
            {
                Sequence = 2,
                Description = "Add Quote and Charges",
                EventCode = "DDI",
                ActualDate = DateTime.Now.ToString(),
                EstimatedDate = DateTime.Now.ToString(),
                ShipmentId = shipment,
            },
            new ShipmentMilestone()
            {
                Sequence = 3,
                Description = "Follow up on Telex HBL",
                EventCode = "MCR",
                ActualDate = DateTime.Now.ToString(),
                EstimatedDate = DateTime.Now.ToString(),
                ShipmentId = shipment,
            },
            new ShipmentMilestone()
            {
                Sequence = 4,
                Description = "Add Pre Alert to eDocs",
                EventCode = "RFW",
                ActualDate = DateTime.Now.ToString(),
                EstimatedDate = DateTime.Now.ToString(),
                ShipmentId = shipment,
            },
            new ShipmentMilestone()
            {
                Sequence = 5,
                Description = "Sending Customs set up Documents to Agent",
                EventCode = "DDI",
                ActualDate = DateTime.Now.ToString(),
                EstimatedDate = DateTime.Now.ToString(),
                ShipmentId = shipment,
            },
            new ShipmentMilestone()
            {
                Sequence = 6,
                Description = "Send Cartage Advice to Transporter",
                EventCode = "RFW",
                ActualDate = DateTime.Now.ToString(),
                EstimatedDate = DateTime.Now.ToString(),
                ShipmentId = shipment,
            },
            new ShipmentMilestone()
            {
                Sequence = 7,
                Description = "Billing/Invoice",
                EventCode = "JED",
                ActualDate = DateTime.Now.ToString(),
                EstimatedDate = DateTime.Now.ToString(),
                ShipmentId = shipment,
            },
            new ShipmentMilestone()
            {
                Sequence = 8,
                Description = "Send Final Cartage Advice to Transporter",
                EventCode = "ADD",
                ActualDate = DateTime.Now.ToString(),
                EstimatedDate = DateTime.Now.ToString(),
                ShipmentId = shipment,
            },
            new ShipmentMilestone()
            {
                Sequence = 9,
                Description = "Proof of Empty Return",
                EventCode = "ADD",
                ActualDate = DateTime.Now.ToString(),
                EstimatedDate = DateTime.Now.ToString(),
                ShipmentId = shipment,
            },
            new ShipmentMilestone()
            {
                Sequence = 10,
                Description = "Delivered",
                EventCode = "DLV",
                ActualDate = DateTime.Now.ToString(),
                EstimatedDate = DateTime.Now.ToString(),
                ShipmentId = shipment,
            }
        };
    }

    private static IEnumerable<User> GetUsers()
    {
        return new User[]
        {
            new User()
            {
                Name = "Ankit Kumar",
                UserName = "ankit@sflhub.com",
                Email = "ankit@sflhub.com",
                EmailConfirmed = true,
                Password = "G00gle@123#",
                Organizations = "TST1"
            },
            new User()
            {
                Name = "Avnish R",
                UserName = "avnish@sflhub.com",
                Email = "avnish@sflhub.com",
                EmailConfirmed = true,
                Password = "G00gle@123#",
                Organizations = "TST3"
            },
            new User()
            {
                Name = "Krishna Kalyana",
                UserName = "krishna@sflhub.com",
                Email = "krishna@sflhub.com",
                EmailConfirmed = true,
                Password = "G00gle@123#",
                Organizations = "TST5"
            },
        };
    }

    private static IEnumerable<Event> GetEvents()
    {
        return new Event[]
        {
            new Event()
            {
                EventModuleId = "S00001234",
                EventModuleType = "SHP",
                EventType = "Milestone",
                EventDesctipion = "Delivered",
                EventData = DateTime.Now,
                IsDevliverd = false,
            },
            new Event()
            {
                EventModuleId = "S00001345",
                EventModuleType = "SHP",
                EventType = "Milestone",
                EventDesctipion = "Send Cartage Advice to Transporter",
                EventData = DateTime.Now,
                IsDevliverd = false,
            },
            new Event()
            {
                EventModuleId = "S00001554",
                EventModuleType = "SHP",
                EventType = "Milestone",
                EventDesctipion = "Add Pre Alert to eDocs",
                EventData = DateTime.Now,
                IsDevliverd = false,
            },
            new Event()
            {
                EventModuleId = "S00001744",
                EventModuleType = "SHP",
                EventType = "Milestone",
                EventDesctipion = "Follow up on Telex HBL",
                EventData = DateTime.Now,
                IsDevliverd = false,
            },
            new Event()
            {
                EventModuleId = "S00001894",
                EventModuleType = "SHP",
                EventType = "Milestone",
                EventDesctipion = "Follow up on Telex HBL",
                EventData = DateTime.Now,
                IsDevliverd = false,
            }
        };
    }
}
