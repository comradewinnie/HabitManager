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
                return response.IsSuccessStatusCode;  // MI ieteica izmantot šo, ja metode atgriež bool
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CreateHabitAsync error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteHabitAsync(Guid id)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/habits/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DeleteHabitAsync error: {ex.Message}");
                return false;
            }
        }

        public async Task<HabitDto?> GetHabitAsync(Guid id)
        {
            try
            {
                var response = await _http.GetAsync($"api/habits/{id}");

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;  // ja nav šāda ieraduma
                }

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<HabitDto>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetHabitAsync error: {ex.Message}");
                return null;
            }
        }

        public async Task<IReadOnlyList<CompletionRecordDto>?> GetHabitCompletionHistoryAsync(Guid id)
        {
            try
            {
                var response = await _http.GetAsync($"api/habits/{id}/completion-history");

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;  // ja nav šāda ieraduma
                }

                response.EnsureSuccessStatusCode();

                var history = await response.Content.ReadFromJsonAsync<List<CompletionRecordDto>>()
                              ?? new List<CompletionRecordDto>();

                return history;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetHabitCompletionHistoryAsync error: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<HabitDto>?> GetUserHabitsAsync(string userId)
        {
            try
            {
                var response = await _http.GetAsync($"api/users/{userId}/habits");

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;  // ja nav šāda lietotāja
                }

                response.EnsureSuccessStatusCode();

                var habits = await response.Content.ReadFromJsonAsync<List<HabitDto>>()
                             ?? new List<HabitDto>();

                return habits;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetUserHabitsAsync error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> MarkAsCompleteAsync(MarkHabitAsCompleteDto dto)
        {
            try
            {
                var response = await _http.PostAsJsonAsync($"api/habits/{dto.HabitId}/complete", dto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"MarkAsCompleteAsync error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ToggleHabitStatusAsync(Guid id)
        {
            try
            {
                var response = await _http.PutAsync($"api/habits/{id}/toggle-status", null);  // null, tāpēc ka nepadodam nekādus objektus
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ToggleHabitStatusAsync error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateHabitAsync(Guid id, UpdateHabitDto dto)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"api/habits/{id}", dto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"UpdateHabitAsync error: {ex.Message}");
                return false;
            }
        }
    }
}