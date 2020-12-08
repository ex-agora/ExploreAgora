/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 *  You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 *  otherwise make available to any third party the Service or the Content. */

#if SIS_IAP
using UnityEngine.Purchasing;
#endif
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using SIS.SimpleJSON;

namespace SIS
{
    public class IAPEditorExporter
    {
        public static List<IAPCurrency> FromJSON(string text)
        {
            List<IAPCurrency> currency = new List<IAPCurrency>();
            JSONNode data = JSON.Parse(text);
            
            for (int i = 0; i < data.Count; i++)
            {
                IAPCurrency cur = new IAPCurrency();
                cur.name = data[i]["DisplayName"].Value;
                cur.amount = data[i]["InitialDeposit"].AsInt;
                currency.Add(cur);
            }

            return currency;
        }
        
        
        public static List<IAPGroup> FromJSON(string text, List<IAPCurrency> currencies)
        {
            List<IAPGroup> IAPs = new List<IAPGroup>();
            JSONNode data = JSON.Parse(text)["Catalog"];
            
            for (int i = 0; i < data.Count; i++)
            {
                IAPGroup g = IAPs.SingleOrDefault(x => x.name == data[i]["SIS"]["Group"]["Name"].Value);
                if (g == null)
                {
                    g = new IAPGroup();
                    g.id = data[i]["SIS"]["Group"]["Id"].Value;
                    g.name = data[i]["SIS"]["Group"]["Name"].Value;
                    g.items = new List<IAPObject>();
                    IAPs.Add(g);
                }
                
                IAPObject obj = new IAPObject();
                obj.id = data[i]["ItemId"].Value;
                obj.title = data[i]["DisplayName"].Value;
                obj.description = data[i]["Description"].Value;
                obj.type = (ProductType)data[i]["SIS"]["Type"].AsInt;
                obj.realPrice = data[i]["SIS"]["Price"].Value;
                obj.fetch = data[i]["SIS"]["Fetch"].AsBool;
                obj.icon = (Sprite)AssetDatabase.LoadAssetAtPath(data[i]["SIS"]["Icon"].Value, typeof(Sprite));
                
                if (!string.IsNullOrEmpty(data[i]["Bundle"].ToString()))
                {
                    obj.editorType = IAPType.Currency;
                    obj.virtualPrice = new List<IAPCurrency>();
                    
                    foreach (string key in data[i]["Bundle"]["BundledVirtualCurrencies"].AsObject.Keys)
                    {
                        for (int j = 0; j < currencies.Count; j++)
                        {
                            if (currencies[j].name.StartsWith(key, System.StringComparison.OrdinalIgnoreCase))
                            {
                                IAPCurrency cur = new IAPCurrency();
                                cur.name = currencies[j].name;
                                cur.amount = data[i]["Bundle"]["BundledVirtualCurrencies"][key].AsInt;
                                obj.virtualPrice.Add(cur);
                                break;
                            }
                        }
                    }
                }
                else if (string.IsNullOrEmpty(data[i]["VirtualCurrencyPrices"]["RM"].Value))
                {
                    obj.editorType = IAPType.Virtual;
                    obj.virtualPrice = new List<IAPCurrency>();
                    
                    foreach (string key in data[i]["VirtualCurrencyPrices"].AsObject.Keys)
                    {
                        for (int j = 0; j < currencies.Count; j++)
                        {
                            if (currencies[j].name.StartsWith(key, System.StringComparison.OrdinalIgnoreCase))
                            {
                                IAPCurrency cur = new IAPCurrency();
                                cur.name = currencies[j].name;
                                cur.amount = data[i]["VirtualCurrencyPrices"][key].AsInt;
                                obj.virtualPrice.Add(cur);
                                break;
                            }
                        }
                    }
                }
                
                if (obj.editorType != IAPType.Virtual)
                {
                    int platformCount = System.Enum.GetValues(typeof(IAPPlatform)).Length;
                    for (int j = 0; j < platformCount; j++)
                    {                        
                        if (string.IsNullOrEmpty(data[i]["SIS"]["PlatformId"][j.ToString()].Value))
                            continue;

                        obj.storeIDs.Add(new StoreID(((IAPPlatform)j).ToString(), data[i]["SIS"]["PlatformId"][j.ToString()].Value));
                    }
                }
                
                if (!string.IsNullOrEmpty(data[i]["SIS"]["Requirement"].ToString()))
                {
                    obj.req.entry = data[i]["SIS"]["Requirement"]["Id"].Value;
                    obj.req.target = data[i]["SIS"]["Requirement"]["Value"].AsInt;
                    obj.req.labelText = data[i]["SIS"]["Requirement"]["Text"].Value;
                    obj.req.nextId = data[i]["SIS"]["Requirement"]["Next"].Value;
                }
                
                g.items.Add(obj);
            }
            
            return IAPs;
        }


        public static string ToJSON(List<IAPCurrency> currency)
        {
            JSONNode data = new JSONClass();
            JSONArray curArray = new JSONArray();
            
            for (int i = 0; i < currency.Count; i++)
            {
                IAPCurrency cur = currency[i];

                curArray[i]["CurrencyCode"] = new JSONData(cur.name.Substring(0, 2).ToUpper());
                curArray[i]["DisplayName"] = new JSONData(cur.name);
                curArray[i]["InitialDeposit"] = new JSONData(cur.amount);
            }
            
            data = curArray;
            return data.ToString();
        }
            

        public static string ToJSON(List<IAPGroup> IAPs, bool playfabVersion = false)
        {
            JSONNode data = new JSONClass();
            data["CatalogVersion"] = new JSONData(1);
            JSONArray itemArray = new JSONArray();

            for (int i = 0; i < IAPs.Count; i++)
            {
                for (int j = 0; j < IAPs[i].items.Count; j++)
                {
                    IAPObject obj = IAPs[i].items[j];
                    JSONNode node = new JSONClass();
                    node["CatalogVersion"] = new JSONData(1);
					node["ItemId"] = new JSONData(obj.id);
					node["DisplayName"] = new JSONData(obj.title);
                    node["Description"] = new JSONData(obj.description);

                    switch (obj.type)
                    {
                        case ProductType.Consumable:
                            node["Consumable"]["UsagePeriod"] = new JSONData(5);
                            if (obj.editorType != IAPType.Currency) node["IsStackable"] = new JSONData(true);
                            break;

                        #if SIS_IAP
                        case ProductType.Subscription:
                            node["IsStackable"] = new JSONData(true);
                            break;
                        #endif
                    }

                    switch (obj.editorType)
                    {
                        case IAPType.Default:
                        case IAPType.Currency:
                            string allowedChars = "01234567890.,";
                            string realPrice = new string(obj.realPrice.Where(c => allowedChars.Contains(c)).ToArray());
                            double price = 0;

                            if (!string.IsNullOrEmpty(realPrice))
                            {
                                double.TryParse(realPrice, out price);
                                price *= 100;
                            }
                            node["VirtualCurrencyPrices"]["RM"] = new JSONData(price);

                            if (obj.editorType == IAPType.Currency)
                            {
                                for (int k = 0; k < obj.virtualPrice.Count; k++)
                                {
                                    node["Bundle"]["BundledVirtualCurrencies"][obj.virtualPrice[k].name.Substring(0, 2).ToUpper()] = new JSONData(obj.virtualPrice[k].amount);
                                }
                            }
                            break;

                        case IAPType.Virtual:
                            bool isFree = true;
                            for (int k = 0; k < obj.virtualPrice.Count; k++)
                            {
                                node["VirtualCurrencyPrices"][obj.virtualPrice[k].name.Substring(0, 2).ToUpper()] = new JSONData(obj.virtualPrice[k].amount);
                                if (obj.virtualPrice[k].amount > 0) isFree = false;
                            }

                            if (playfabVersion && isFree) continue;
                            break;
                    }

                    node["SIS"]["Group"]["Name"] = new JSONData(IAPs[i].name);
                    node["SIS"]["Group"]["Id"] = new JSONData(IAPs[i].id);
                    node["SIS"]["Fetch"] = new JSONData(obj.fetch);
                    node["SIS"]["Price"] = new JSONData(obj.realPrice);
                    node["SIS"]["Type"] = new JSONData((int)obj.type);
                    if(obj.icon) node["SIS"]["Icon"] = new JSONData(AssetDatabase.GetAssetPath(obj.icon));

                    if (!string.IsNullOrEmpty(obj.req.entry))
                    {
                        node["SIS"]["Requirement"]["Id"] = new JSONData(obj.req.entry);
                        node["SIS"]["Requirement"]["Value"] = new JSONData(obj.req.target);
                        node["SIS"]["Requirement"]["Text"] = new JSONData(obj.req.labelText);
                    }
                    if (!string.IsNullOrEmpty(obj.req.nextId))
                    {
                        node["SIS"]["Requirement"]["Next"] = new JSONData(obj.req.nextId);
                    }

                    for (int k = 0; k < obj.storeIDs.Count; k++)
                    {
                        string platformId = obj.storeIDs[k].id;
                            
                        if (playfabVersion)
                        {
                            JSONNode platformNode = JSON.Parse(node.ToString());
                            platformNode["ItemId"] = new JSONData(platformId);

                            if(itemArray.Childs.Count(x => x["ItemId"].Value == platformId) == 0)
                                itemArray[itemArray.Count] = platformNode;
                            continue;
                        }

						int platform = (int)((IAPPlatform)System.Enum.Parse(typeof(IAPPlatform), obj.storeIDs[k].store));
						node["SIS"]["PlatformId"][platform.ToString()] = new JSONData(platformId);
                    }

                    itemArray[itemArray.Count] = node;
                }
            }

            data["Catalog"] = itemArray;
            return data.ToString();
        }
    }
}
