using MathSpace.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MathSpace.Tool
{
   public class DeserailizeHelper
    {
        public static Type GetXmlNodeType(string nodeName)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var allTypes = currentAssembly.GetTypes();
            foreach (var typeItem in allTypes)
            {
                if (typeItem.Name == nodeName)
                {
                    return typeItem;
                }
            }

            return null;
        }

        public static void DeserializeBlockNode(XmlNode xmlBlockNode, BlockNode node)
        {
            foreach (XmlNode item in xmlBlockNode.ChildNodes)
            {
                var itemType = GetXmlNodeType(item.Name);
                if (null != itemType)
                {
                    var itemObj = Activator.CreateInstance(itemType);
                    if (itemObj is CharactorBlock)
                    {
                        foreach (var charItem in item.InnerText)
                        {
                            CharactorBlock charactiorBlock = FontManager.CreateNewCharactorBlock(charItem);
                            node.Children.Add(charactiorBlock);
                            charactiorBlock.ParentId = node.ID;
                        }
                    }
                    else if (itemObj is Fraction)
                    {
                        Fraction fraction = itemObj as Fraction;
                        node.Children.Add(fraction);
                        fraction.ParentId = node.ID;
                        foreach (XmlNode fractionItem in item.ChildNodes)
                        {
                            if (fractionItem.Name == "Denominator")
                            {
                                fraction.Denominator = new BlockNode();
                                DeserializeBlockNode(fractionItem, fraction.Denominator);
                            }
                            else if (fractionItem.Name == "Molecule")
                            {
                                fraction.Molecule = new BlockNode();
                                DeserializeBlockNode(fractionItem, fraction.Molecule);
                            }
                        }
                    }
                    else if (itemObj is Exponential)
                    {
                        Exponential exponential = itemObj as Exponential;
                        node.Children.Add(exponential);
                        exponential.ParentId = node.ID;
                        foreach (XmlNode exponentialItem in item.ChildNodes)
                        {
                            if (exponentialItem.Name == "Index")
                            {
                                exponential.Index = new BlockNode();
                                DeserializeBlockNode(exponentialItem, exponential.Index);

                            }
                            else if (exponentialItem.Name == "Base")
                            {
                                exponential.Base = new BlockNode();
                                DeserializeBlockNode(exponentialItem, exponential.Base);
                            }
                        }
                    }
                    else if (itemObj is Radical)
                    {
                        Radical radical = itemObj as Radical;
                        node.Children.Add(radical);
                        radical.ParentId = node.ID;
                        foreach (XmlNode itemRadicalChild in item.ChildNodes)
                        {
                            if (itemRadicalChild.Name == "RootIndex")
                            {
                                radical.RootIndex = new BlockNode();
                                DeserializeBlockNode(itemRadicalChild, radical.RootIndex);

                            }
                            else if (itemRadicalChild.Name == "Radicand")
                            {
                                radical.Radicand = new BlockNode();
                                DeserializeBlockNode(itemRadicalChild, radical.Radicand);
                            }
                        }
                    }
                    else if (itemObj is BlockNode)
                    {
                        BlockNode blockNode = itemObj as BlockNode;
                        node.Children.Add(blockNode);
                        blockNode.ParentId = node.ID;
                        DeserializeBlockNode(item, blockNode);
                    }
                }
            }
        }
    }
}
