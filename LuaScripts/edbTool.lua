local edb = GetModData().Buildings
local edu = GetModData().Units
local buildingCount = edb:GetCount()

---@class recruitedUnit
---@field unit string
---@field cityPools table<integer, Capability>
---@field castlePools table<integer, Capability>
---@field bothPools table<integer, Capability>
recruitedUnit = {}

function recruitedUnit:new(o)
    o = o or {}
    o.unit = o.unit or ""
    o.cityPools = o.cityPools or {}
    o.castlePools = o.castlePools or {}
    o.bothPools = o.bothPools or {}
    o.unitValue = o.unitValue or 0
    o.unitValuePerSoldier = o.unitValuePerSoldier or 0
    setmetatable(o, self)
    self.__index = self
    return o
end

---@type table<string, recruitedUnit>
RECRUIT_UNITs = {}

function getPoolData()
    print("Start getting pool data..")
    for i = 0, buildingCount - 1 do
        local building = edb:GetByIndex(i)
        if building then
            for j = 0, building.LevelCount - 1 do
                local level = building.Levels[j]
                for c = 0, level.CapabilityCount - 1 do
                    local capability = level.Capabilities[c]
                    if capability.Type == "recruit_pool" and capability.MaximumPool >= 1 then
                        local unit = capability.Unit
                        if not RECRUIT_UNITs[unit] then
                            RECRUIT_UNITs[unit] = recruitedUnit:new({ unit = unit })
                        end

                        if level.AvailableCastle and level.AvailableCity then
                            table.insert(RECRUIT_UNITs[unit].bothPools, capability)
                        elseif level.AvailableCastle then
                            table.insert(RECRUIT_UNITs[unit].castlePools, capability)
                        else
                            table.insert(RECRUIT_UNITs[unit].cityPools, capability)
                        end
                    end
                end
            end
        end
    end
    print("Finish getting pool data..")
end

function sortRates(rateTable)
    table.sort(rateTable, function(a, b)
        return a < b
    end)
end

function makeCSV()
    print("Start Making csv..")
    local csv = io.open("recruit_pool.csv", "w")
    if not csv then
        print("Cant open file")
        return
    end
    csv:write("Unit;Average;Median;AverageCity;MedianCity;AverageCastle;MedianCastle;Highest;Lowest;UnitValue\n")
    for k, v in pairs(RECRUIT_UNITs) do
        local bothRates = {}
        local cityRates = {}
        local castleRates = {}
        local both = 0
        local city = 0
        local castle = 0
        local bothCount = 0
        local cityCount = 0
        local castleCount = 0
        local highest = 0
        local lowest = 9999
        local highestStr = ""
        local lowestStr = ""
        for i = 1, #v.bothPools do
            table.insert(bothRates, v.bothPools[i].ReplenishmentRate)
            both = both + v.bothPools[i].ReplenishmentRate
            bothCount = bothCount + 1
            if v.bothPools[i].ReplenishmentRate > highest then
                highest = v.bothPools[i].ReplenishmentRate
                highestStr = v.bothPools[i].ReplenishmentRateString
            end
            if v.bothPools[i].ReplenishmentRate < lowest then
                lowest = v.bothPools[i].ReplenishmentRate
                lowestStr = v.bothPools[i].ReplenishmentRateString
            end
        end
        for i = 1, #v.cityPools do
            table.insert(bothRates, v.cityPools[i].ReplenishmentRate)
            table.insert(cityRates, v.cityPools[i].ReplenishmentRate)
            city = city + v.cityPools[i].ReplenishmentRate
            cityCount = cityCount + 1
            if v.cityPools[i].ReplenishmentRate > highest then
                highest = v.cityPools[i].ReplenishmentRate
                highestStr = v.cityPools[i].ReplenishmentRateString
            end
            if v.cityPools[i].ReplenishmentRate < lowest then
                lowest = v.cityPools[i].ReplenishmentRate
                lowestStr = v.cityPools[i].ReplenishmentRateString
            end
        end
        for i = 1, #v.castlePools do
            table.insert(bothRates, v.castlePools[i].ReplenishmentRate)
            table.insert(castleRates, v.castlePools[i].ReplenishmentRate)
            castle = castle + v.castlePools[i].ReplenishmentRate
            castleCount = castleCount + 1
            if v.castlePools[i].ReplenishmentRate > highest then
                highest = v.castlePools[i].ReplenishmentRate
                highestStr = v.castlePools[i].ReplenishmentRateString
            end
            if v.castlePools[i].ReplenishmentRate < lowest then
                lowest = v.castlePools[i].ReplenishmentRate
                lowestStr = v.castlePools[i].ReplenishmentRateString
            end
        end
        local average = (both + city + castle) / (bothCount + cityCount + castleCount)
        local averageCity = 0
        local median = bothRates[math.ceil(bothCount + cityCount + castleCount / 2)]
        local medianCity = 0
        local medianCastle = 0
        sortRates(bothRates)
        sortRates(cityRates)
        sortRates(castleRates)
        if cityCount > 0 then
            averageCity = city / cityCount
            medianCity = cityRates[math.ceil(#cityRates / 2)]
        end
        local averageCastle = 0
        if castleCount > 0 then
            averageCastle = castle / castleCount
            medianCastle = castleRates[math.ceil(#castleRates / 2)]
        end

        local un = edu:Get(k)
        local unitValue = 0
        if un then
            unitValue = un.AiUnitValue
        end
        csv:write(k ..
            ";" ..
            average ..
            ";" ..
            median ..
            ";" ..
            averageCity .. ";" .. medianCity .. ";" .. averageCastle .. ";" .. medianCastle .. ";" .. highestStr ..
            ";" .. lowestStr .. ";" .. unitValue .. "\n")
    end
    csv:flush()
    csv:close()
    print("Finish Making csv..")
end

getPoolData()
makeCSV()
