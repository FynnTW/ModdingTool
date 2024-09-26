local modData = GetModData()
local bmdb = modData.BattleModelDb

unitTypes = {
    infantry = {
        mace = "clanfolk_fighters",
        sword = "dw_avari_ambusher",
        sword2H = "dw_civguard",
        spear = "dw_avari_defender",
        halberd = "dw_reformed_halberd",
        crossbow = {
            mace = "dw_thorn_xbow"
        },
        archer = {
            sword = "dw_avari_archer",
            mace = "dw_kugath_nobles",
        },
        javelin = {
            spear = "dw_leof_clanband",
            mace = "dw_leof_retainers",
            sword = "dw_mariners",
        }
    },
    cavalry = {
        javelin = {
            spear = "dw_leof_far_riders",
        },
        lance = {
            sword = "dw_reformed_lancer",
        },
        mace = "hillsteed_riders"
    },
    mount = {
        "mount_dw_lancer_horse"
    }
}

UNITS_FOLDER = "unit_models/_units/"

function createEntry(name, mesh, texture, textureNorm, attach, attachNorm, type)
    local base = bmdb:Get(type)
    if not base then return end
    local entry = base:CloneEntry(name)
    entry.LodTable[0].Mesh = mesh
    setAllTexture(entry, texture, textureNorm)
    if attach then
        setAllAttachTexture(entry, attach, attachNorm)
    end
    bmdb:Add(entry)
end

function setAllTexture(bmdbEntry, texture, norm, sprite)
    for m = 0, bmdbEntry.MainTexturesCount - 1 do
        bmdbEntry.MainTextures[m].TexturePath = texture
        bmdbEntry.MainTextures[m].Normal = norm
        if sprite then
            bmdbEntry.MainTextures[m].Sprite = sprite
        end
    end
end

function setAllAttachTexture(bmdbEntry, texture, norm, sprite)
    for m = 0, bmdbEntry.AttachTexturesCount - 1 do
        bmdbEntry.AttachTextures[m].TexturePath = texture
        bmdbEntry.AttachTextures[m].Normal = norm
        if sprite then
            bmdbEntry.AttachTextures[m].Sprite = sprite
        end
    end
end

function isExistingFaction(faction, existingFactions)
    for _, existingFaction in ipairs(existingFactions) do
        if faction == existingFaction then
            return true
        end
    end
    return false
end

local factions = {
    "aztecs",
    "teutonic_order",
    "normans",
    "portugal",
    "hungary",
    "hre",
    "turks",
    "norway",
    "timurids",
    "mongols",
    "moors",
    "ireland",
    "saxons",
    "england",
    "poland",
    "venice",
    "spain",
    "russia",
    "sicily",
    "denmark",
    "slave",
    "merc",
    "khand",
    "france",
    "byzantium",
    "scotland",
    "gundabad"
}
function setAllFactionsInBmdb()
    local modelNum = bmdb:GetCount()
    for i = 0, modelNum - 1 do
        local model = bmdb:GetByIndex(i)
        local existingFactions = {}
        if model.MainTexturesCount > 0 then
            local exampleEntry = model.MainTextures[0]
            for m = 0, model.MainTexturesCount - 1 do
                local texture = model.MainTextures[m]
                if texture.Faction == "slave" then
                    exampleEntry = texture
                end
                table.insert(existingFactions, texture.Faction)
            end
            for _, faction in ipairs(factions) do
                if not isExistingFaction(faction, existingFactions) then
                    model:AddMainTexture(faction, exampleEntry.TexturePath, exampleEntry.Normal, exampleEntry.Sprite, model.Name)
                end
            end
        end
        existingFactions = {}
        if model.AttachTexturesCount > 0 then
            local exampleEntry = model.AttachTextures[0]
            for m = 0, model.AttachTexturesCount - 1 do
                local texture = model.AttachTextures[m]
                if texture.Faction == "slave" then
                    exampleEntry = texture
                end
                table.insert(existingFactions, texture.Faction)
            end
            for _, faction in ipairs(factions) do
                if not isExistingFaction(faction, existingFactions) then
                    model:AddAttachTexture(faction, exampleEntry.TexturePath, exampleEntry.Normal, exampleEntry.Sprite, model.Name)
                end
            end
        end
    end
end


createEntry(
    "clanfolk_fighters3", 
    UNITS_FOLDER .. "Dunland_Bill/clanfolk_warriors_lod0.mesh",
    UNITS_FOLDER .. "Dunland_Bill/textures/clanfolk.texture",
    UNITS_FOLDER .. "Dunland_Bill/textures/clanfolk_norm.texture",
    UNITS_FOLDER .. "Dunland_Bill/textures/clanfolk_attach.texture",
    UNITS_FOLDER .. "Dunland_Bill/textures/clanfolk_attach_norm.texture",
    unitTypes.infantry.mace
)
--setAllFactionsInBmdb()