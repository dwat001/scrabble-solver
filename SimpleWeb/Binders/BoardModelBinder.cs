using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using Kakariki.Scrabble.Logic;
using Kakariki.Scrabble.SimpleWeb.Models;

namespace Kakariki.Scrabble.SimpleWeb.Binder
{
    public class BoardModelBinder : IModelBinder
    {
        public Task BindModelAsync (ModelBindingContext bindingContext)
        {
            return null;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            bindingContext.ThrowIfNull("BindingContext");
            controllerContext.ThrowIfNull("ControlContext");

            var request = controllerContext.HttpContext.Request;
            WordList list = WordListConfig.Lists[WordListConfig.ENGLISH_AS_A_SECOND_LANGUAGE];
            var board = Board.InitiliseBoard(list);
            for(int row = Board.BOARD_START_INDEX; row <= Board.BOARD_END_INDEX; row++)
            {
                for(int column = Board.BOARD_START_INDEX; column <= Board.BOARD_END_INDEX; column++)
                {
                    string letter = GetA<string>(bindingContext, string.Format("[{0}][{1}].Cell.Letter", row, column));
                    if (!letter.IsNullOrEmpty())
                    {
                        board.GetCell(column, row).Letter = letter[0];
                    }
                }
            }
            return new BoardModel(board, list);
        }

        private T GetA<T>(ModelBindingContext bindingContext, string key) where T : class
        {
            if (String.IsNullOrEmpty(key)) return null;
            ValueProviderResult valueResult;
            //Try it with the prefix...
            string modelName = bindingContext.ModelName + key;
            valueResult = bindingContext.ValueProvider.GetValue(modelName);
            //Didn't work? Try without the prefix if needed...
            //TODO Do we kill the following?
            //if (valueResult == null && bindingContext.FallbackToEmptyPrefix == true)
            if (valueResult == null)
            {
                modelName = key;
                valueResult = bindingContext.ValueProvider.GetValue(modelName);
            }
            if (valueResult == null)
            {
                return null;
            }
            // Add the value to the model state, without this line, a null reference exception is thrown
            // when redisplaying the form with a not convertible value
            bindingContext.ModelState.SetModelValue(modelName, valueResult);
            try
            {
                return (T)Convert.ChangeType(valueResult.FirstValue, typeof(T));
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError(modelName, ex, bindingContext.ModelMetadata);
                return null;
            }
        }

    }
}