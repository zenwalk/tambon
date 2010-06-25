﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace De.AHoerstemeier.Tambon
{
    public enum EntityType
    {
        Unknown,
        Changwat,
        Amphoe, KingAmphoe,
        Tambon,
        Muban,
        Thesaban, // actually not a type, but a mistake in DOPA population of Nong Khai where they forgot the type
        ThesabanNakhon, ThesabanMueang, ThesabanTambon,
        Khet,
        Khwaeng,
        SakhaTambon, SakhaKhwaeng, Sakha,
        Monthon,
        Sukhaphiban,
        SukhaphibanTambon, SukhaphibanMueang,
        Mueang, Bangkok,
        Chumchon,
        TAO,
        TC,
        PAO,
        Phak, KlumChangwat
    };
    public enum EntityModification
    {
        Creation,
        Abolishment,
        Rename,
        StatusChange,
        AreaChange,
        Constituency
    }
    public enum ProtectedAreaTypes
    {
        NationalPark,
        ForestPark,
        WildlifeSanctuary,
        NonHuntingArea,
        HistoricalPark,
        HistoricalSite,
        NationalPreservedForest
    }
    public enum EntityLeaderType
    {
        Unknown,
        Governor,
        ViceGovernor,
        DistrictOfficer,
        MinorDistrictOfficer,
        Kamnan,
        PhuYaiBan,
        PAOChairman,
        Mayor,
        TAOChairman,
        SanitaryDistrictChairman
    }
    public enum PersonTitle
    {
        Unknown,
        Mister,
        Miss,
        Mistress,
        General,
        LieutenantGeneral,
        MajorGeneral,
        Colonel,
        LieutenantColonel,
        Major, 
        Captain,
        FirstLieutenant,
        SecondLieutenant,
        ActingSecondLieutenant,
        SubLieutenant
    }
    public enum OfficeType
    {
        Unknown,
        ProvinceHall,
        PAOOffice,
        DistrictOffice,
        TAOOffice,
        MunicipalityOffice,
        VillageHeadmanOffice,
        DistrictMuseum // ???
    }
    public enum GazetteSignPosition
    { 
        Unknown,
        PrimeMinister,
        MinisterOfInterior,
        DeputyMinisterOfInterior,
        MinistryOfInteriorPermanentSecretary,
        ProvinceGovernor,
        BangkokGovernor,
        BangkokPermanentSecretary,
        DeputyBangkokPermanentSecretary,
        MinisterOfInformationAndCommunicationTechnology,
        ElectionCommissionPresident
    }
}