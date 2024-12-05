local edb = GetModData().Buildings
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

function makeCSV()
    print("Start Making csv..")
    local csv = io.open("recruit_pool.csv", "w")
    if not csv then
        print("Cant open file")
        return
    end
    csv:write("Unit;Average;AverageCity;AverageCastle;Highest;Lowest\n")
    for k, v in pairs(RECRUIT_UNITs) do
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
        if cityCount > 0 then
            averageCity = city / cityCount
        end
        local averageCastle = 0
        if castleCount > 0 then
            averageCastle = castle / castleCount
        end
        csv:write(k .. ";" .. average .. ";" .. averageCity .. ";" .. averageCastle .. ";" .. highestStr ..
            ";" .. lowestStr .. "\n")
    end
    csv:flush()
    csv:close()
    print("Finish Making csv..")
end

getPoolData()
makeCSV()
edb:WriteFile()
