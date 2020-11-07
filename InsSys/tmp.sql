SELECT hpe.*, sd.*, dd.*, ddt.*, pe.*, pep.*, ins.* from [Nbp].[HeaderPolicyElements] hpe
	left join [Nbp].[SourceDocuments] sd on sd.Id = hpe.SourceDocumentId
	right join [Nbp].[DocumentDescriptors] dd on dd.DocumentId = sd.Id
	left join [Nbp].[DocumentDescriptorTypes] ddt on dd.DescriptorTypeId = ddt.Id
	left join [Nbp].[PolicyElements] pe on hpe.PolicyElementId = pe.Id
	left join [Nbp].[Policies] pep on pe.PolicyId = pep.Id
	left join [Nbp].[Insurers] ins on pep.InsurerId = ins.Id