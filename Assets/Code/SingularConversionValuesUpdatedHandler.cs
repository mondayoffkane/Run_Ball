using System;
using Newtonsoft.Json;

public interface SingularConversionValuesUpdatedHandler {
    void OnConversionValuesUpdated(int value, int coarse, bool _lock);
}
