using HabitManager.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;

namespace HabitManager.Maui.Data
{
    public class HabitApiService : IHabitProvider  // adaptēts pēc "UzdevumuParvaldnieks" piemēra
    {
        private readonly HttpClient _http;

        public HabitApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> CreateHabitAsync(CreateHabitDto dto)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/habits", dto);

                if (response.IsSuccessStatusCode)  // ja bija izveidots
                    return true;

                var errorMsg = await response.Content.ReadAsStringAsync();  // ja nebija izveidots
                errorMsg = errorMsg.Trim('"');  // MI ieteica šo, lai nebūu pēdiņu sākumā un beigās, jo serveris atgriež kļūdu kā JSON
                throw new InvalidOperationException(errorMsg);
            }
            catch (HttpRequestException ex)  // ja problēmas ar tīklu
            {
                Debug.WriteLine($"CreateHabitAsync error: {ex.Message}");
                throw new InvalidOperationException("Network error. Please check your internet connection.");
            }
            catch (TaskCanceledException ex)  // ja serveris neatbild
            {
                Debug.WriteLine($"CreateHabitAsync error: {ex.Message}");
                throw new InvalidOperationException("Server unavailable. Please try later.");
            }
        }

        public async Task<bool> DeleteHabitAsync(Guid id)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/habits/{id}");

                if (response.IsSuccessStatusCode)
                    return true;

                var errorMsg = await response.Content.ReadAsStringAsync();
                errorMsg = errorMsg.Trim('"');
                throw new InvalidOperationException(errorMsg);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"CreateHabitAsync error: {ex.Message}");
                throw new InvalidOperationException("Network error. Please check your internet connection.");
            }
            catch (TaskCanceledException ex)
            {
                Debug.WriteLine($"CreateHabitAsync error: {ex.Message}");
                throw new InvalidOperationException("Server unavailable. Please try later.");
            }
        }

        public async Task<HabitDto> GetHabitAsync(Guid id)
        {
            try
            {
                var response = await _http.GetAsync($"api/habits/{id}");

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<HabitDto>();

                var errorMsg = await response.Content.ReadAsStringAsync();
                errorMsg = errorMsg.Trim('"');
                throw new InvalidOperationException(errorMsg);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"CreateHabitAsync error: {ex.Message}");
                throw new InvalidOperationException("Network error. Please check your internet connection.");
            }
            catch (TaskCanceledException ex)
            {
                Debug.WriteLine($"CreateHabitAsync error: {ex.Message}");
                throw new InvalidOperationException("Server unavailable. Please try later.");
            }
        }

        public async Task<IReadOnlyList<CompletionRecordDto>> GetHabitCompletionHistoryAsync(Guid id)
        {
            try
            {
                var response = await _http.GetAsync($"api/habits/{id}/completion-history");

                if (response.IsSuccessStatusCode)  // ja saņēmām
                {
                    var history = await response.Content.ReadFromJsonAsync<List<CompletionRecordDto>>()
                              ?? new List<CompletionRecordDto>();

                    return history;
                }

                var errorMsg = await response.Content.ReadAsStringAsync();  // ja nesaņēmām
                errorMsg = errorMsg.Trim('"');
                throw new InvalidOperationException(errorMsg);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"CreateHabitAsync error: {ex.Message}");
                throw new InvalidOperationException("Network error. Please check your internet connection.");
            }
            catch (TaskCanceledException ex)
            {
                Debug.WriteLine($"CreateHabitAsync error: {ex.Message}");
                throw new InvalidOperationException("Server unavailable. Please try later.");
            }
        }

        public async Task<IEnumerable<HabitDto>> GetUserHabitsAsync(string userId)
        {
            try
            {
                var response = await _http.GetAsync($"api/users/{userId}/habits");

                if (response.IsSuccessStatusCode)
                {
                    var habits = await response.Content.ReadFromJsonAsync<List<HabitDto>>()
                             ?? new List<HabitDto>();

                    return habits;
                }

                var errorMsg = await response.Content.ReadAsStringAsync();
                errorMsg = errorMsg.Trim('"');
                throw new InvalidOperationException(errorMsg);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"CreateHabitAsync error: {ex.Message}");
                throw new InvalidOperationException("Network error. Please check your internet connection.");
            }
            catch (TaskCanceledException ex)
            {
                Debug.WriteLine($"CreateHabitAsync error: {ex.Message}");
                throw new InvalidOperationException("Server unavailable. Please try later.");
            }
        }

        public async Task<bool> MarkAsCompleteAsync(MarkHabitAsCompleteDto dto)
        {
            try
            {
                var response = await _http.PostAsJsonAsync($"api/habits/{dto.HabitId}/complete", dto);

                if (response.IsSuccessStatusCode)
                    return true;

                var errorMsg = await response.Content.ReadAsStringAsync();
                errorMsg = errorMsg.Trim('"');
                throw new InvalidOperationException(errorMsg);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"CreateHabitAsync error: {ex.Message}");
                throw new InvalidOperationException("Network error. Please check your internet connection.");
            }
            catch (TaskCanceledException ex)
            {
                Debug.WriteLine($"CreateHabitAsync error: {ex.Message}");
                throw new InvalidOperationException("Server unavailable. Please try later.");
            }
        }

        public async Task<bool> ToggleHabitStatusAsync(Guid id)
        {
            try
            {
                var response = await _http.PutAsync($"api/habits/{id}/toggle-status", null);  // null, tāpēc ka nepadodam nekādus objektus

                if (response.IsSuccessStatusCode)
                    return true;

                var errorMsg = await response.Content.ReadAsStringAsync();
                errorMsg = errorMsg.Trim('"');
                throw new InvalidOperationException(errorMsg);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"CreateHabitAsync error: {ex.Message}");
                throw new InvalidOperationException("Network error. Please check your internet connection.");
            }
            catch (TaskCanceledException ex)
            {
                Debug.WriteLine($"CreateHabitAsync error: {ex.Message}");
                throw new InvalidOperationException("Server unavailable. Please try later.");
            }
        }

        public async Task<bool> UpdateHabitAsync(Guid id, UpdateHabitDto dto)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"api/habits/{id}", dto);

                if (response.IsSuccessStatusCode)
                    return true;

                var errorMsg = await response.Content.ReadAsStringAsync();
                errorMsg = errorMsg.Trim('"');
                throw new InvalidOperationException(errorMsg);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"CreateHabitAsync error: {ex.Message}");
                throw new InvalidOperationException("Network error. Please check your internet connection.");
            }
            catch (TaskCanceledException ex)
            {
                Debug.WriteLine($"CreateHabitAsync error: {ex.Message}");
                throw new InvalidOperationException("Server unavailable. Please try later.");
            }
        }
    }
}