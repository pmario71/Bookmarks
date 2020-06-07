using Bookmarks.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmarks.Pages
{
    public class FilterStringValidator : ComponentBase
    {
        private ValidationMessageStore _messageStore;

        [CascadingParameter] EditContext CurrentEditContext { get; set; }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            if (CurrentEditContext == null)
            {
                throw new InvalidOperationException($"{nameof(FilterStringValidator)} requires a cascading " +
                    $"parameter of type {nameof(EditContext)}. For example, you can use {nameof(FilterStringValidator)} " +
                    $"inside an {nameof(EditForm)}.");
            }

            _messageStore = new ValidationMessageStore(CurrentEditContext);
            CurrentEditContext.OnValidationRequested += (s, e) => 
            {
                Console.WriteLine("Validation requested ...");
                _messageStore.Clear();
                var model = CurrentEditContext.Model as BookmarkSvc;

                try
                {
                    model.ApplyFilter();
                }
                catch(ArgumentOutOfRangeException ex)
                {
                    _messageStore.Add(CurrentEditContext.Field(nameof(BookmarkSvc.Filter)), ex.Message);
                }
                CurrentEditContext.NotifyValidationStateChanged();
            };

            CurrentEditContext.OnFieldChanged += (s, e) => 
            { 
                _messageStore.Clear(e.FieldIdentifier);

                var model = CurrentEditContext.Model as BookmarkSvc;

                try
                {
                    model.ApplyFilter();
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    _messageStore.Add(CurrentEditContext.Field(nameof(BookmarkSvc.Filter)), ex.Message);
                }
                CurrentEditContext.NotifyValidationStateChanged();
            };
        }

        public void DisplayErrors(Dictionary<string, List<string>> errors)
        {
            foreach (var err in errors)
            {
                _messageStore.Add(CurrentEditContext.Field(err.Key), err.Value);
            }
            CurrentEditContext.NotifyValidationStateChanged();
        }
    }

}
